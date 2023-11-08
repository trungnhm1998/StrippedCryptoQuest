using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    [Serializable]
    public class PassiveProvideTurnBasePolicy : IGameplayEffectPolicy
    {
        [SerializeField] private PassiveAbility _passiveAbility;
        public PassiveAbility PassiveAbility => _passiveAbility;

        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new PassiveProvideTurnBasePolicyActiveEffect(this, inSpec);
    }

    /// <summary>
    /// This action depends on <see cref="CastSkillAbility"/> in order to work.
    /// </summary>
    [Serializable]
    public class PassiveProvideTurnBasePolicyActiveEffect : ActiveGameplayEffect
    {
        [SerializeField] private int _turnsLeft;
        private const int DEFAULT_TURNS = 1;

        private PassiveProvideTurnBasePolicy _policyDef;
        private Battle.Components.Character _character;
        private readonly TinyMessageSubscriptionToken _unloadingEvent;
        private GameplayAbilitySpec _ability;
        private AbilitySystemBehaviour _abilitySystem;

        public PassiveProvideTurnBasePolicyActiveEffect(PassiveProvideTurnBasePolicy policyDef,
            GameplayEffectSpec spec) : base(spec)
        {
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            _policyDef = policyDef;
            _turnsLeft = (context == null || context.Turns == 0) ? DEFAULT_TURNS : context.Turns;
            _character = Spec.Target.GetComponent<Battle.Components.Character>();
            _abilitySystem = _character.AbilitySystem;

            _character.TurnEnded += CheckTurn;
            spec.Target.TagSystem.TagAdded += ExpiredEffectWhenTargetDie;
            _unloadingEvent = BattleEventBus.SubscribeEvent<UnloadingEvent>(ExpiredEffect);
        }

        ~PassiveProvideTurnBasePolicyActiveEffect()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            _character.TurnEnded -= CheckTurn;
            Spec.Target.TagSystem.TagAdded -= ExpiredEffectWhenTargetDie;
            BattleEventBus.UnsubscribeEvent(_unloadingEvent);
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
            _turnsLeft = 0;
            RemoveAbility();
            Spec.Target.EffectSystem.RemoveEffect(Spec);
            RemoveEvents();
        }

        private void TryGiveAbility()
        {
            if (_abilitySystem.FindAbilitySpecFromDef(_policyDef.PassiveAbility) != null) return;
            _ability = _abilitySystem.GiveAbility(_policyDef.PassiveAbility);
        }

        private void CheckTurn()
        {
            if (_turnsLeft <= 0) return;

            TryGiveAbility();
            _turnsLeft--;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(
                $"AbilitySystem::Special turn base[{_character.DisplayName}] [{Spec.Def.name}] has [{_turnsLeft}] turns left");
#endif
            if (_turnsLeft <= 0)
                RemoveAbility();
        }

        private void RemoveAbility()
        {
            IsActive = false; // Will be remove when next turn started
            if (_ability == null) return;
            _abilitySystem.RemoveAbility(_ability);
        }
    }
}