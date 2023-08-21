using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

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
        
        [Tooltip("Ability with these will be hiden in skill panel")]
        [field: SerializeField]
        public TagScriptableObject[] NotActivableSkillTags { get; private set; }

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
                if (CheckUnitDisableWithTag(tag)) return true;    
            }

            return false;
        }

        /// <summary>
        /// Some skill are auto activate
        /// </summary>
        /// <param name="tagToCheck"></param>
        /// <returns></returns>
        public bool CheckNotActivableSkillTag(TagScriptableObject tagToCheck)
        {
            for (var i = 0; i < NotActivableSkillTags.Length; i++)
            {
                if (NotActivableSkillTags[i] == tagToCheck) return true;
            }
            return false;
        }
    }
}