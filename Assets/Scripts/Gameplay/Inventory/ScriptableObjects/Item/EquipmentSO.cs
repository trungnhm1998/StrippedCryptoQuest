using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Variant;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Crypto Quest/Inventory/Equipment Item")]
    public class EquipmentSO : ItemGenericSO
    {
        [field: Header("Equipment Item")]
        [field: SerializeField] public EquipmentTypeSO EquipmentType { get; protected set; }

        [field: SerializeField] public VariantBaseSO VariantBase { get; private set; }
        [field: SerializeField] public RaritySO Rarity { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedEquipmentType { get; private set; }
        [field: SerializeField] public int RequiredCharacterLevel { get; private set; }
        [FormerlySerializedAs("_stats")]
        [SerializeField, Header("Attributes")] private StatsDef _statsDef;
        public StatsDef StatsDef => _statsDef;


        #region EDITOR

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

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor"/>
        /// </summary>
        /// <param name="variantBase"></param>
        public void Editor_SetVariantBase(VariantBaseSO variantBase)
        {
            VariantBase = variantBase;
        }

        /// <summary>
        /// This method will be use in <see cref="CryptoQuestEditor.Gameplay.Inventory.UsableSOEditor"/>
        /// </summary>
        /// <param name="localizedEquipmentType"></param>
        public void Editor_SetLocalizedEquipmentType(LocalizedString localizedEquipmentType)
        {
            LocalizedEquipmentType = localizedEquipmentType;
        }
#endif

        #endregion
    }
}