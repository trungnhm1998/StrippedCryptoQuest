using System;
using CryptoQuest.Item.Inventory;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ItemInfomation
    {
        public int Quantity;
        public ItemSO ItemSO;

        public ItemInfomation(ItemSO itemSO, int quantity = 0)
        {
            ItemSO = itemSO;
            Quantity = quantity;
        }
    }
}
