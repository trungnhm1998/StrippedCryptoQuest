using System.Collections;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Ability that will give the owner an effect on granted. 
    ///
    /// This will use with <see cref="EquipmentInfo"/>, the equipment will have a passive ability
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Simple Passive Ability", fileName = "Passive")]
    public class PassiveAbility : AbilityScriptableObject<PassiveAbilitySpec>
    {
        [field: SerializeField] public int Id { get; private set; }

        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        
        private void OnValidate()
        {
            Id = int.Parse(name);
        }
    }

    public class PassiveAbilitySpec : GameplayAbilitySpec
    {
        protected GameplayEffectContext SkillContext { get; private set; }
        protected Battle.Components.Character Character { get; private set; }

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            Character = owner.GetComponent<Battle.Components.Character>();
            SkillContext = ((PassiveAbility)abilitySO).Context;
        }

        public override void OnAbilityGranted(GameplayAbilitySpec gameplayAbilitySpec)
        {
            base.OnAbilityGranted(gameplayAbilitySpec);
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            yield break;
        }
    }
}