using System;
using CryptoQuest.Data;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ItemInformation
    {
        protected ItemGenericSO _item;
        public ItemGenericSO Item => _item;

        protected string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ItemInformation(ItemGenericSO item)
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