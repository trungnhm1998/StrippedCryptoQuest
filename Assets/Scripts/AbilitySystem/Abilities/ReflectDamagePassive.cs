using System.Collections;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class ReflectDamagePassive : PassiveAbility
    {
        [field: SerializeField] public GameplayEffectDefinition ReflectDamageEffect { get; private set; }
        [field: SerializeField] public float ReflectDamagePercentage { get; private set; } = 30f;

        protected override GameplayAbilitySpec CreateAbility()
            => new ReflectDamagePassiveSpec();
    }

    public class ReflectDamagePassiveSpec : PassiveAbilitySpec
    {
        private readonly ReflectDamagePassive _def;
        private TinyMessageSubscriptionToken _receivedDamageEvent;

        public ReflectDamagePassiveSpec() => _def = (ReflectDamagePassive)AbilitySO;

        protected override IEnumerator OnAbilityActive()
        {
            _receivedDamageEvent = BattleEventBus.SubscribeEvent<ReceivedPhysicalDamageEvent>(ReflectDamage);
            yield break;
        }

        private void ReflectDamage(ReceivedPhysicalDamageEvent ctx)
        {
            if (ctx.Receiver != Character) return;
            var contextHandle = new GameplayEffectContextHandle(new ReflectContext(ctx.Receiver, ctx.Dealer, ctx.Damage,
                _def.ReflectDamagePercentage));
            var effectSpec = _def.ReflectDamageEffect.CreateEffectSpec(Owner, contextHandle);
            ctx.Dealer.GameplayEffectSystem.ApplyEffectToSelf(effectSpec);
        }

        protected override void OnAbilityEnded() => BattleEventBus.UnsubscribeEvent(_receivedDamageEvent);

        ~ReflectDamagePassiveSpec() => OnAbilityEnded();
    }
}