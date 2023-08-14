using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class UsableSO : ItemGenericSO
    {
        [field: Header("Usable Item")]
        [field: SerializeField] public UsableTypeSO UsableTypeSO { get; private set; }
        [field: SerializeField] public ActionDefinitionBase ActionDefinition { get; private set; }

        [field: SerializeField] public AbilityScriptableObject Ability { get; private set; }
        
        public ActionSpecificationBase Action => ActionDefinition.Create();


#if UNITY_EDITOR

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor.ImportBatchData"/> 
        /// </summary>
        /// <param name="usableType"></param>
        public void Editor_SetUsableType(UsableTypeSO usableType)
        {
            UsableTypeSO = usableType;
        }
#endif
    }
}