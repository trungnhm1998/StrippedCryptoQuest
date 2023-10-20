using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Item
{
    [Serializable]
    public abstract class ItemInfo
    {
        [SerializeField, ReadOnly] private long _id = -1;

        public long Id
        {
            get => _id;
            set => _id = value;
        }

        public abstract int Price { get; }
        public abstract int SellPrice { get; }

        public abstract bool IsValid();
    }

    [Serializable]
    public abstract class ItemInfo<TDef> : ItemInfo where TDef : GenericItem
    {
        [field: SerializeField] public TDef Data { get; set; } // TODO: Primitive item ID instead

        protected ItemInfo(TDef baseGenericItem)
        {
            Data = baseGenericItem;
        }

        protected ItemInfo() { }

        public override bool IsValid() => Data != null;
    }
}