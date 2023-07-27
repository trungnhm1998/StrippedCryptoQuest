using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

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
