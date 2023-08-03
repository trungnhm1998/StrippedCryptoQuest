using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public enum EInventoryActionType
    {
        Use = 0, // For materials, consumables, stackable items, ...etc
        Equip = 1, // For Weapon,  Gem ,NFT, ...etc
    }

    [CreateAssetMenu(fileName = "ItemType", menuName = "Crypto Quest/Inventory/ItemType")]
    public class ItemTypeSO
    {
        public Sprite Icon;
        [SerializeField] private EInventoryActionType _actionType;

        [SerializeField] private ItemTypeSO _parentItemType;

        public ItemTypeSO ParentType => _parentItemType;
        public EInventoryActionType ActionType => _actionType;
    }
}