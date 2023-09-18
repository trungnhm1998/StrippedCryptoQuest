using System.Collections;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    public class NormalAttackAbility : AbilityScriptableObject<NormalAttackAbilitySpec>
    {
        [field: SerializeField] public GameplayEffectDefinition NormalAttackEffect { get; private set; }
    }

    public class NormalAttackAbilitySpec : GameplayAbilitySpec
    {
        private NormalAttackAbility _normalAttackAbility;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            _normalAttackAbility = abilitySO as NormalAttackAbility;
        }

        private IDamageable _target;

        public void Execute(IDamageable damageable)
        {
            _target = damageable;
            TryActiveAbility();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            // play some effect by raise event 
            var effectSpec = Owner.MakeOutgoingSpec(_normalAttackAbility.NormalAttackEffect);
            _target.ReceiveDamage(effectSpec);
            yield break;
        }
    }
}