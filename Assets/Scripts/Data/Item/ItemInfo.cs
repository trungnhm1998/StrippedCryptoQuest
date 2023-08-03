using System;

namespace CryptoQuest.Data.Item
{
    [Serializable]
    public class ItemInfo
    {
        protected ItemData _item;
        public ItemData Item => _item;

        protected string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public int Quantity = 0;

        public ItemInfo(ItemData item, int quantity)
        {
            _item = item;
            _id = Guid.NewGuid().ToString();
            Quantity = quantity;
        }

        public ItemInfo()
        {
            _item = null;
            _id = Guid.NewGuid().ToString();
            Quantity = 1;
        }

        public bool IsValid()
        {
            return _item != null && Quantity > 0;
        }
    }

    [Serializable]
    public class RandomItemInfo : ItemInfo
    {
        public int Weight;

        public RandomItemInfo(ItemData item, int quantity) : base(item, quantity) { }
    }
}