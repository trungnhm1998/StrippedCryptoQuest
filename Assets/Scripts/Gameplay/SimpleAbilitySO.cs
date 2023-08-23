using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class SimpleAbilitySO : AbilityScriptableObject
    {
        [field: SerializeField] public EffectScriptableObject Effect { get; private set; }

        protected override GameplayAbilitySpec CreateAbility()
        {
            var ability = new SimpleGameplayAbilitySpec
            {
                Effect = Effect
            };
            return ability;
        }
    }

    public class SimpleGameplayAbilitySpec : GameplayAbilitySpec
    {
        public EffectScriptableObject Effect { get; set; }
        private Character _target;
        private AbilitySystemBehaviour _targetSystem;

        public void Active(Character target)
        {
            _target = target;
            _targetSystem = target.GameplayAbilitySystem;
            
            TryActiveAbility();
        }

        protected override IEnumerator InternalActiveAbility()
        {
            GameplayEffectSpec effectSpecSpec = Owner.MakeOutgoingSpec(Effect);
            _targetSystem.ApplyEffectSpecToSelf(effectSpecSpec);
            yield break;
        }
    }
}