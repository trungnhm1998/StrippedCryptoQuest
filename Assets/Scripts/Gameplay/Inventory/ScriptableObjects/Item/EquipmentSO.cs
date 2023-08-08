using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Crypto Quest/Inventory/Equipment Item")]
    public class EquipmentSO : ItemGenericSO
    {
        [field: Header("Equipment Item")]
        [field: SerializeField] public EquipmentTypeSO EquipmentType { get; protected set; }

        [field: SerializeField] public RaritySO Rarity { get; private set; }

#if UNITY_EDITOR

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor"/> 
        /// </summary>
        /// <param name="equipmentType"></param>
        public void Editor_SetEquipmentType(EquipmentTypeSO equipmentType)
        {
            EquipmentType = equipmentType;
        }

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor"/> 
        /// </summary>
        /// <param name="rarity"></param>
        public void Editor_SetRarity(RaritySO rarity)
        {
            Rarity = rarity;
        }
#endif
    }
}