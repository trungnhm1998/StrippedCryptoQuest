﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Item
{
    [Serializable]
    public abstract class ItemInfo
    {
        [SerializeField] private string _id;

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