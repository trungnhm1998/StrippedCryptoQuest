using CryptoQuest.Item.Inventory;
using UnityEngine;

namespace CryptoQuest
{
    public struct ItemInformation
    {
        public Sprite Icon;
        public string NameItem;
        public string Description;
        public int Quantity;

        public ItemInformation(ItemSO itemSO)
        {
            this.Icon = itemSO.Icon;
            this.NameItem = itemSO.Name;
            this.Description = itemSO.Description;
            this.Quantity = 1;
        }
    }
}
