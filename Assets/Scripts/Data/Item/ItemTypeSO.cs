using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public enum EInventoryActionType
    {
        DoNothing = 0,
        Use = 1, // For materials, consumables, stackable items, ...etc
        Equip = 2, // For Weapon, Camera, Gem ,NFT, ...etc
        InGameUse = 3, // For items that can be used in game ex: ocarina, ...etc
    }

    [CreateAssetMenu(fileName = "ItemType", menuName = "Crypto Quest/Inventory/ItemType")]
    public class ItemTypeSO : ItemGenericData
    {
        public Sprite Icon;
        [SerializeField] private EInventoryActionType actionType;

        [Tooltip("The category of the item. Used for sorting in the inventory.")]
        [SerializeField] private InventoryCategorySO category;

        [SerializeField] ItemTypeSO parentItemType;

        public ItemTypeSO ParentType => parentItemType;
        public EInventoryActionType ActionType => actionType;

        public InventoryCategorySO Category
        {
            get
            {
                if (parentItemType != null)
                {
                    return parentItemType.Category;
                }

                return category;
            }
        }
    }
}