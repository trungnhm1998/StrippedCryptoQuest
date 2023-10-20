using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Item
{
    [Serializable]
    public abstract class ItemInfo
    {
        [SerializeField, ReadOnly] private string _id = Guid.NewGuid().ToString();

        public string Id
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