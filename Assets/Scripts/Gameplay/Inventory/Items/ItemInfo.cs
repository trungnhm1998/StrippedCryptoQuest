using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
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