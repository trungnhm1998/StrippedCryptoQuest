using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public enum EInventoryCategoryType
    {
        Other = 0,
        Customization = 1, // For equipment, weapons, ...etc
        Material = 2,
        Artifact = 3,
    }

    [CreateAssetMenu(fileName = "InventoryCategoryType", menuName = "Crypto Quest/Inventory/Inventory Category Type")]
    public class InventoryCategorySO : ItemGenericData
    {
        [SerializeField] private EInventoryCategoryType _categoryType;
        public EInventoryCategoryType CategoryType => _categoryType;
    }
}