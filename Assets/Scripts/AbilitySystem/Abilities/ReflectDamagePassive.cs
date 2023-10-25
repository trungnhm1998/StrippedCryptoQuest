using System.Collections;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class ReflectDamagePassive : PassiveAbility
    {
        [field: SerializeField] public DealingDamageEvent PhysicalDamageEvent { get; private set; }
        [field: SerializeField] public GameplayEffectDefinition ReflectDamageEffect { get; private set; }
        [field: SerializeField] public float ReflectDamagePercentage { get; private set; } = 30f;

        protected override GameplayAbilitySpec CreateAbility()
            => new ReflectDamagePassiveSpec();
    }

    public class ReflectDamagePassiveSpec : PassiveAbilitySpec
    {
        private ReflectDamagePassive _def;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            _def = (ReflectDamagePassive)AbilitySO;
        }

        protected override IEnumerator OnAbilityActive()
        {
            _def.PhysicalDamageEvent.DamageDealt += ReflectDamage;
            yield break;
        }

        private void ReflectDamage(Battle.Components.Character dealer, Battle.Components.Character receiver,
            float damage)
        {
            if (receiver != Character) return;
            BattleEventBus.RaiseEvent(new ReflectDamageEvent { Character = receiver });
            var gameplayEffectContext = new ReflectContext(receiver, dealer, damage, _def.ReflectDamagePercentage);
            var contextHandle = new GameplayEffectContextHandle(gameplayEffectContext);
            var effectSpec = _def.ReflectDamageEffect.CreateEffectSpec(Owner, contextHandle);
            dealer.GameplayEffectSystem.ApplyEffectToSelf(effectSpec);
        }

        protected override void OnAbilityEnded() => _def.PhysicalDamageEvent.DamageDealt -= ReflectDamage;
    }
}