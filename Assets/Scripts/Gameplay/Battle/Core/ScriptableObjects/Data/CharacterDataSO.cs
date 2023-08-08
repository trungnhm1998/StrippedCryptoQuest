using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Gameplay/Character/Character Data")]
    public class CharacterDataSO : InitializeAttributeDatabase
    {
        public string Name;
        public AbilityScriptableObject NormalAttack;
        public AbilityScriptableObject GuardAbilitySO;
        public AbilityScriptableObject RetreatAbilitySO;
        public List<AbilityScriptableObject> GrantedAbilities;

        /// <summary>
        /// Need to create character info when there're
        /// many characters using the same data set, like enemies
        /// </summary>
        /// <returns></returns>
        public CharacterInformation CreateCharacterInfo()
        {
            return new CharacterInformation(this, Name);
        }
    }

    public class CharacterInformation
    {
        public AbilitySystemBehaviour Owner { get; set; }
        public CharacterDataSO Data { get; private set; }
        public string OriginalName { get; private set; }
        public string DisplayName { get; set; }

        public CharacterInformation(CharacterDataSO data, string originalName)
        {
            Data = data;
            OriginalName = originalName; 
            DisplayName = originalName;
        }
    }
}
