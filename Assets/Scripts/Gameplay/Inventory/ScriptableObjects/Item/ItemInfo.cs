using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public abstract class ItemInfo
    {
        public ItemGenericSO BaseItem { get; protected set; }
        public string Id { get; set; }

        [field: SerializeField] public bool IsNftItem { get; private set; }

        protected ItemInfo(ItemGenericSO baseItem)
        {
            BaseItem = baseItem;
            Id = Guid.NewGuid().ToString();
        }

        protected ItemInfo()
        {
            BaseItem = null;
            Id = Guid.NewGuid().ToString();
        }

        public bool IsValid()
        {
            return BaseItem != null;
        }

        protected abstract void Activate();
    }
}