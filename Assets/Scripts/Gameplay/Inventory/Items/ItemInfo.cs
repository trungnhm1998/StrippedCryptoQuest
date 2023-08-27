using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    [Serializable]
    public abstract class ItemInfo<TDef> where TDef : GenericItem
    {
        public string Id { get; }

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

        public virtual bool IsValid()
        {
            return Data != null;
        }

        protected abstract void Activate();
    }
}