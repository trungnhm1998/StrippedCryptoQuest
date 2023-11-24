using System;
using CryptoQuest.Gameplay.Inventory;
using UnityEngine.Serialization;

namespace CryptoQuest.Item
{
    [Serializable]
    public abstract class ItemInfo
    {
        public abstract bool IsValid();
        public abstract bool AddToInventory(IInventoryController inventory);
        public abstract bool RemoveFromInventory(IInventoryController inventory);
    }

    [Serializable]
    public abstract class ItemInfo<TDef> : ItemInfo where TDef : GenericItem
    {
        // TODO: Primitive item ID instead
        [FormerlySerializedAsAttribute("<Data>k__BackingField")]
        public TDef Data;

        protected ItemInfo(TDef baseGenericItem)
        {
            Data = baseGenericItem;
        }

        protected ItemInfo() { }

        public override bool IsValid() => Data != null;
    }
}