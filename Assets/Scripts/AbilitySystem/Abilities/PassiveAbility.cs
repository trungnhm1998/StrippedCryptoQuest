using System.Collections;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Ability that will give the owner an effect on granted. 
    ///
    /// This will use with <see cref="EquipmentInfo"/>, the equipment will have a passive ability
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive Ability", fileName = "Passive")]
    public class PassiveAbility : AbilityScriptableObject
    {
        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        [SerializeField] private GameplayEffectDefinition _effect; // Should be infinite type

        /// <summary>
        /// Effect that will give to the system when this ability is granted
        /// </summary>
        public GameplayEffectDefinition PassiveEffect => _effect;

        protected override GameplayAbilitySpec CreateAbility()
        {
            return new PassiveAbilitySpec(this);
        }
    }

    public class PassiveAbilitySpec : GameplayAbilitySpec
    {
        private readonly PassiveAbility _passiveAbility;
        private GameplayEffectSpec _effectSpec;
        private ActiveGameplayEffect _activeEffectSpec;

        public PassiveAbilitySpec(PassiveAbility passiveAbility)
        {
            _passiveAbility = passiveAbility;
        }

        public override void OnAbilityGranted(GameplayAbilitySpec gameplayAbilitySpec)
        {
            base.OnAbilityGranted(gameplayAbilitySpec);
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            if (_passiveAbility.PassiveEffect == null) yield break;
            _effectSpec = _passiveAbility.PassiveEffect.CreateEffectSpec(Owner, new GameplayEffectContextHandle(_passiveAbility.Context));
            _activeEffectSpec = Owner.ApplyEffectSpecToSelf(_effectSpec);
        }

        /// <summary>
        /// When un equip the equipment, remove the ability
        /// </summary>
        protected override void OnAbilityEnded() => Owner.EffectSystem.RemoveEffect(_effectSpec);
    }
}