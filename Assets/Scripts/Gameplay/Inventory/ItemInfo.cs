﻿using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public abstract class ItemInfo
    {
        public ItemGenericSO BaseItem { get; protected set; }
        public string Id { get; set; }

        [field: SerializeField] public bool IsNftItem { get; private set; }

        public ItemInfo(ItemGenericSO baseItem)
        {
            BaseItem = baseItem;
            Id = Guid.NewGuid().ToString();
        }

        public ItemInfo()
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