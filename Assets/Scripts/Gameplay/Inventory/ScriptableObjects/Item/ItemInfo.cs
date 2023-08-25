using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public abstract class ItemInfo<TDef> where TDef : GenericItem
    {
        public string Id { get; set; }

        [field: SerializeField] public TDef Data { get; private set; } // TODO: Primitive item ID instead
        [field: SerializeField] public bool IsNftItem { get; private set; }

        protected ItemInfo(TDef baseGenericItem) : this()
        {
            Data = baseGenericItem;
        }

        protected ItemInfo()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool IsValid()
        {
            return Data != null;
        }

        protected abstract void Activate();
    }
}