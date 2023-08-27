using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class UsableSO : GenericItem
    {
        [field: Header("Usable Item")]
        [field: SerializeField] public UsableTypeSO UsableTypeSO { get; private set; }
        [field: SerializeField] public ActionDefinitionBase ActionDefinition { get; private set; }

        [field: SerializeField] public SimpleAbilitySO Ability { get; private set; }
        
        public ActionSpecificationBase Action => ActionDefinition.Create();


#if UNITY_EDITOR
        /// <summary>
        /// Make sure ability of this item has item's name
        /// to show in battle when select item
        /// </summary>
        private void OnValidate()
        {
            if (Ability == null) return;
        }

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