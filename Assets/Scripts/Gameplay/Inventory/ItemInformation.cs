using System;
using CryptoQuest.Data;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ItemInformation
    {
        public ItemGeneric Data;

        protected Data.Item.Item _item;
        public Data.Item.Item Item => _item;

        protected string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ItemInformation(Data.Item.Item item, int quantity)
        {
            _item = item;
            _id = Guid.NewGuid().ToString();
        }

        public ItemInformation()
        {
            _item = null;
            _id = Guid.NewGuid().ToString();
        }

        public bool IsValid()
        {
            return _item != null;
        }
    }
}