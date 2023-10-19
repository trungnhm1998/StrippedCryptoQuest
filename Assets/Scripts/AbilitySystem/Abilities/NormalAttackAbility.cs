using System.Collections;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class NormalAttackAbility : AbilityScriptableObject
    {
        [field: SerializeField] public GameplayEffectDefinition NormalAttackEffect { get; private set; }
        protected override GameplayAbilitySpec CreateAbility() => new NormalAttackAbilitySpec(this);
    }

    public class NormalAttackAbilitySpec : GameplayAbilitySpec
    {
        private readonly NormalAttackAbility _normalAttackAbility;
        private IDamageable _target;

        public NormalAttackAbilitySpec(NormalAttackAbility normalAttackAbility)
        {
            _normalAttackAbility = normalAttackAbility;
        }

        public void Execute(IDamageable damageable)
        {
            _target = damageable;
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            // play some effect by raise event 
            var effectSpec = Owner.MakeOutgoingSpec(_normalAttackAbility.NormalAttackEffect);
            _target.ReceiveDamage(effectSpec);
            EndAbility();
            yield break;
        }
    }
}