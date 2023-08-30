using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    [Serializable]
    public abstract class ItemInfo<TDef> where TDef : GenericItem
    {
        [SerializeField, ReadOnly] private string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        [field: SerializeField] public TDef Data { get; private set; } // TODO: Primitive item ID instead
        [field: SerializeField] public bool IsNftItem { get; private set; }

        protected ItemInfo(TDef baseGenericItem) : this()
        {
            Data = baseGenericItem;
        }

        protected ItemInfo()
        {
            _id = Guid.NewGuid().ToString();
        }

        public virtual bool IsValid()
        {
            return Data != null;
        }

        protected abstract void Activate();
    }
}