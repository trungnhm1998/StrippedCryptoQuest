using System.Collections;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Apply an instant effect to owner when the action is done
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Post Action With Effect Passive", fileName = "PostActionPassive", order = 0)]
    public class PostActionPassiveAbility : PassiveAbility
    {
        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        [SerializeField] private GameplayEffectDefinition _effect; // Should be infinite type
        public GameplayEffectDefinition EffectToApply => _effect;

        protected override GameplayAbilitySpec CreateAbility()
            => new PostActionPassiveAbilitySpec(this);
    }

    public class PostActionPassiveAbilitySpec : PassiveAbilitySpec
    {
        private readonly PostActionPassiveAbility _def;
        private TinyMessageSubscriptionToken _turnEndedEvent;

        public PostActionPassiveAbilitySpec(PostActionPassiveAbility def) => _def = def;
        
        ~PostActionPassiveAbilitySpec() => BattleEventBus.UnsubscribeEvent(_turnEndedEvent);

        protected override IEnumerator OnAbilityActive()
        {
            if (_def.Context.Parameters.targetAttribute.Attribute == null) yield break;
            _turnEndedEvent = BattleEventBus.SubscribeEvent<TurnEndedEvent>(ApplyEffectPostAction);
        }

        private void ApplyEffectPostAction(TurnEndedEvent ctx)
        {
            var contextHandle = new GameplayEffectContextHandle(_def.Context);
            var effectSpec = _def.EffectToApply.CreateEffectSpec(Owner, contextHandle);
            Owner.ApplyEffectSpecToSelf(effectSpec);
        }

        protected override void OnAbilityEnded() => BattleEventBus.UnsubscribeEvent(_turnEndedEvent);
    }
}