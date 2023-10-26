using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Passive With Effect", fileName = "PassiveWithEffectAbility")]
    public class PassiveWithEffectAbility : PassiveAbility
    {
        [SerializeField] private GameplayEffectDefinition _effect; // Should be infinite type

        /// <summary>
        /// Effect that will give to the system when this ability is granted
        /// </summary>
        public GameplayEffectDefinition PassiveEffect => _effect;

        protected override GameplayAbilitySpec CreateAbility() => new PassiveWithEffectAbilitySpec(this);
    }

    public class PassiveWithEffectAbilitySpec : PassiveAbilitySpec
    {
        private readonly PassiveWithEffectAbility _def;
        private GameplayEffectSpec _effectSpec;
        private ActiveGameplayEffect _activeEffectSpec;

        public PassiveWithEffectAbilitySpec(PassiveWithEffectAbility abilityDef) => _def = abilityDef;

        protected override IEnumerator OnAbilityActive()
        {
            if (_def.PassiveEffect == null) yield break;
            var contextHandle = new GameplayEffectContextHandle(SkillContext);
            _effectSpec = _def.PassiveEffect.CreateEffectSpec(Owner, contextHandle);
            _activeEffectSpec = Owner.ApplyEffectSpecToSelf(_effectSpec);
        }

        /// <summary>
        /// When un equip the equipment, remove the ability
        /// </summary>
        protected override void OnAbilityEnded() => Owner.EffectSystem.RemoveEffect(_effectSpec);
    }
}