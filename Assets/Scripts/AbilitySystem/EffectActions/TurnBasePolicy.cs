using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
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

        [SerializeField] private int _turn = 1;
        public int Turn => _turn;
        [SerializeField] private ETriggerType _triggerType = ETriggerType.Always;
        public ETriggerType TriggerType => _triggerType;

        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new TurnBasePolicyActiveEffect(this, inSpec);
    }

    /// <summary>
    /// This action depends on <see cref="CastSkillAbility"/> in order to work.
    /// </summary>
    [Serializable]
    public class TurnBasePolicyActiveEffect : ActiveGameplayEffect
    {
        [SerializeField] private int _turnsLeft;

        private TurnBasePolicy _policyDef;
        private Battle.Components.Character _character;
        private DamageOverTimeFlags _damageOverTimeFlagsFlags;

        public TurnBasePolicyActiveEffect(TurnBasePolicy policyDef,
            GameplayEffectSpec spec) : base(spec)
        {
            _policyDef = policyDef;
            _turnsLeft = policyDef.Turn;
            _character = Spec.Target.GetComponent<Battle.Components.Character>();
            if (_character.TryGetComponent(out _damageOverTimeFlagsFlags) == false)
                _damageOverTimeFlagsFlags = _character.gameObject.AddComponent<DamageOverTimeFlags>();
            RegisterWhenEffectWillBeApply();
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

        ~TurnBasePolicyActiveEffect()
        {
            UnregisterWhenEffectWillBeApply();
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

        private void ModifyTargetAttributeIfPeriodic()
        {
            if (_turnsLeft <= 0) return;

            LogAffectingStatus();
            ExecutePeriodicEffect();
            _turnsLeft--;
            Debug.Log(
                $"TurnBaseAction::UpdateTurn::skill effect has [{_turnsLeft}] turns left");
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
            foreach (var grantedTag in Spec.GrantedTags)
            {
                if (_damageOverTimeFlagsFlags.FlagRaised(grantedTag)) return;
                ModifyBaseAttributeValueWithEvaluatedEffectModifiers();
                _damageOverTimeFlagsFlags.RaiseFlag(grantedTag);
            }
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
                InternalExecuteMod(Spec, evalData);
            }

            foreach (var evalData in ComputedModifiers) InternalExecuteMod(Spec, evalData);
        }

        public override void ExecuteActiveEffect()
        {
            if (_policyDef.TriggerType != TurnBasePolicy.ETriggerType.Always) return;
            base.ExecuteActiveEffect();
        }
    }
}