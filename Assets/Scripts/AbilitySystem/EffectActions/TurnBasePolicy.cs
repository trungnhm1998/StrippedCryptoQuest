using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    [Serializable]
    public class TurnBasePolicy : IGameplayEffectPolicy
    {
        public enum ETriggerType
        {
            Always = 0, // Effect will apply as long as it is active
            OnTurnStart = 1, // Apply the effect to the target on turn start
            OnTurnEnd = 2, // Apply the effect to the target on turn end
        }

        [SerializeField] private ETriggerType _triggerType = ETriggerType.Always;
        [field: SerializeField] public bool TriggerInstantlyAfterApplied { get; private set; }
        public ETriggerType TriggerType => _triggerType;

        public virtual ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new TurnBasePolicyActiveEffect(this, inSpec);
    }

    /// <summary>
    /// This action depends on <see cref="CastSkillAbility"/> in order to work.
    /// </summary>
    [Serializable]
    public class TurnBasePolicyActiveEffect : ActiveGameplayEffect
    {
        [SerializeField] private int _turnsLeft;
        private const int DEFAULT_TURNS = 1;

        private TurnBasePolicy _policyDef;
        protected Battle.Components.Character _character;
        private DamageOverTimeFlags _damageOverTimeFlagsFlags;
        private readonly TinyMessageSubscriptionToken _unloadingEvent;

        public TurnBasePolicyActiveEffect(TurnBasePolicy policyDef,
            GameplayEffectSpec spec) : base(spec)
        {
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            _policyDef = policyDef;
            _turnsLeft = (context == null || context.Turns == 0) ? DEFAULT_TURNS : context.Turns;
            _character = Spec.Target.GetComponent<Battle.Components.Character>();
            if (_character.TryGetComponent(out _damageOverTimeFlagsFlags) == false)
                _damageOverTimeFlagsFlags = _character.gameObject.AddComponent<DamageOverTimeFlags>();
            RegisterWhenEffectWillBeApply();
            spec.Target.TagSystem.TagAdded += ExpiredEffectWhenTargetDie;
            _unloadingEvent = BattleEventBus.SubscribeEvent<UnloadingEvent>(ExpiredEffect);
            if (_policyDef.TriggerInstantlyAfterApplied) ModifyTargetAttributeIfPeriodic();
        }

        private void RegisterWhenEffectWillBeApply()
        {
            switch (_policyDef.TriggerType)
            {
                case TurnBasePolicy.ETriggerType.Always:
                case TurnBasePolicy.ETriggerType.OnTurnStart:
                    _character.TurnStarted += ModifyTargetAttributeIfPeriodic;
                    break;
                case TurnBasePolicy.ETriggerType.OnTurnEnd:
                    _character.TurnEnded += ModifyTargetAttributeIfPeriodic;
                    break;
                default:
                    return;
            }
        }

        public override void OnRemoved()
        { 
            _turnsLeft = 0;
            IsActive = false;
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            UnregisterWhenEffectWillBeApply();
            Spec.Target.TagSystem.TagAdded -= ExpiredEffectWhenTargetDie;
            BattleEventBus.UnsubscribeEvent(_unloadingEvent);
        }

        private void UnregisterWhenEffectWillBeApply()
        {
            switch (_policyDef.TriggerType)
            {
                case TurnBasePolicy.ETriggerType.Always:
                case TurnBasePolicy.ETriggerType.OnTurnStart:
                    _character.TurnStarted -= ModifyTargetAttributeIfPeriodic;
                    break;
                case TurnBasePolicy.ETriggerType.OnTurnEnd:
                    _character.TurnEnded -= ModifyTargetAttributeIfPeriodic;
                    break;
                default:
                    return;
            }
        }

        private void ExpiredEffect(UnloadingEvent ctx) => RemoveEffect();

        private void ExpiredEffectWhenTargetDie(TagScriptableObject[] tag)
        {
            if (tag.Length == 0) return;
            if (tag[0] != TagsDef.Dead) return;
            RemoveEffect();
        }

        private void RemoveEffect()
        {
            Spec.Target.EffectSystem.RemoveEffect(Spec);
            OnRemoved();
        }

        private void ModifyTargetAttributeIfPeriodic()
        {
            if (_turnsLeft <= 0) return;

            // TODO: Skip these 2 lines if there is an effect with same def has larger magnitude
            LogAffectingStatus();
            ExecutePeriodicEffect();

            _turnsLeft--;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(
                $"AbilitySystem::[{_character.DisplayName}] [{Spec.Def.name}] has [{_turnsLeft}] turns left");
#endif
            if (_turnsLeft <= 0) IsActive = false; // Will be remove when next turn started
        }

        private void LogAffectingStatus()
        {
            var tagAssetProvider = ServiceProvider.GetService<ITagAssetProvider>();
            var grantedTags = Spec.GrantedTags;
            foreach (var tag in grantedTags)
            {
                if (_damageOverTimeFlagsFlags.FlagRaised(tag))
                    return; // Prevent multiple logs with the same type of DoT
                if (tagAssetProvider.TryGetTagAsset(tag, out var tagAsset) == false) continue;
                if (tagAsset.AffectMessage.IsEmpty) continue;
                BattleEventBus.RaiseEvent(new EffectAffectingEvent()
                {
                    Character = Spec.Target.GetComponent<Battle.Components.Character>(),
                    Tag = tag
                });
            }
        }

        private void ExecutePeriodicEffect()
        {
            /*
             * This is to prevent same turn-based effect to be applied multiple times
             */
            foreach (var grantedTag in Spec.GrantedTags)
            {
                if (_damageOverTimeFlagsFlags.FlagRaised(grantedTag)) return;
                if (_policyDef.TriggerType != TurnBasePolicy.ETriggerType.Always)
                    ModifyBaseAttributeValueWithEvaluatedEffectModifiers();
                _damageOverTimeFlagsFlags.RaiseFlag(grantedTag); // TODO: Use the Effect Def it self could be better because not every effect def has tag
            }

            // In case simple restore
            if (Spec.GrantedTags.Length == 0)
                ModifyBaseAttributeValueWithEvaluatedEffectModifiers();
        }

        private void ModifyBaseAttributeValueWithEvaluatedEffectModifiers()
        {
            for (var index = 0; index < Spec.Def.EffectDetails.Modifiers.Length; index++)
            {
                var modifier = Spec.Def.EffectDetails.Modifiers[index];
                var evalData = new GameplayModifierEvaluatedData()
                {
                    Attribute = modifier.Attribute,
                    OpType = modifier.OperationType,
                    Magnitude = Spec.GetModifierMagnitude(index)
                };
                // TODO: Refactor DRY
                OnDamageOverTimeTaken(evalData);
                InternalExecuteMod(Spec, evalData);
            }

            foreach (var evalData in ComputedModifiers)
            {
                // TODO: Refactor DRY
                // Because internal execute can cause character dead so
                // The damage log is shown after dead log
                OnDamageOverTimeTaken(evalData);
                InternalExecuteMod(Spec, evalData);
            }
        }

        private void OnDamageOverTimeTaken(GameplayModifierEvaluatedData evalData)
        {
            if (evalData.Magnitude > 0) return; // healing
            BattleEventBus.RaiseEvent(new DamageOverTimeEvent()
            {
                Character = _character,
                AffectingAttribute = evalData.Attribute,
                Magnitude = evalData.Magnitude
            });
        }

        public override void ExecuteActiveEffect()
        {
            if (_policyDef.TriggerType != TurnBasePolicy.ETriggerType.Always) return;
            base.ExecuteActiveEffect();
        }
    }
}