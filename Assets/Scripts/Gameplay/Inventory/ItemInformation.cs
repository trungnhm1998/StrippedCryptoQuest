using System;
using CryptoQuest.Data;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ItemInformation
    {
        public ItemGenericSO Item { get; protected set; }
        public string Id { get; set; }

        public ItemInformation(ItemGenericSO item)
        {
            Item = item;
            Id = Guid.NewGuid().ToString();
        }

        public ItemInformation()
        {
            Item = null;
            Id = Guid.NewGuid().ToString();
        }

        public bool IsValid()
        {
            return Item != null;
        }
    }
}