using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "BattleUnitTagConfigSO", menuName = "Gameplay/Battle/Data/Battle Unit Tag Config")]
    public class BattleUnitTagConfigSO : ScriptableObject 
    {
        [Tooltip("If unit has these tags they cant select action or action")]
        [field: SerializeField]
        public TagScriptableObject[] DisableActionTags { get; private set; }

        [Tooltip("Unit will activate abilities with this tag before action")]
        [field: SerializeField]
        public TagScriptableObject BeforeActionTag { get; private set; }
        
        [Tooltip("Unit will activate abilities with this tag after action")]
        [field: SerializeField]
        public TagScriptableObject AfterActionTag { get; private set; }

        public bool CheckUnitDisableWithTag(TagScriptableObject tagToCheck)
        {
            for (var i = 0; i < DisableActionTags.Length; i++)
            {
                if (DisableActionTags[i] == tagToCheck) return true;
            }
            return false;
        }

        public bool CheckUnitDisable(List<TagScriptableObject> tags)
        {
            foreach (var tag in tags)
            {
                return CheckUnitDisableWithTag(tag);    
            }

            return false;
        }
    }
}