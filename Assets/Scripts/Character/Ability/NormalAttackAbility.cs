using System.Collections;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    public class NormalAttackAbility : AbilityScriptableObject<NormalAttackAbilitySpec>
    {
        [field: SerializeField] public InstantGameplayEffect NormalAttackEffect { get; private set; }
    }

    public class NormalAttackAbilitySpec : GameplayAbilitySpec
    {
        private NormalAttackAbility _normalAttackAbility;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            _normalAttackAbility = abilitySO as NormalAttackAbility;
        }

        private ICharacter _target;

        public void Execute(ICharacter enemy)
        {
            _target = enemy;
            TryActiveAbility();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            // play some effect by raise event 
            var effectSpec = Owner.MakeOutgoingSpec(_normalAttackAbility.NormalAttackEffect);
            _target.GameplayEffectSystem.ApplyEffectToSelf(effectSpec);
            yield break;
        }
    }
}