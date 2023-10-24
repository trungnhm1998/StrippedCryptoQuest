using System.Collections;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
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
    public class PassiveAbility : AbilityScriptableObject<PassiveAbilitySpec> { }

    public class PassiveAbilitySpec : GameplayAbilitySpec
    {
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