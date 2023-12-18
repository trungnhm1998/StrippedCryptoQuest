using System.Collections.Generic;
using CryptoQuest.Item.Consumable;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [CreateAssetMenu(menuName = "Create ConsumableInventory", fileName = "ConsumableInventory", order = 0)]
    public class ConsumableInventory : ScriptableObject
    {
        [SerializeField] private List<ConsumableInfo> _items = new();
        public List<ConsumableInfo> Items => _items;

        public void Add(ConsumableSO consumable, int quantity = 1)
        {
            if (quantity <= 0) return;

            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                if (item.Data != consumable) continue;
                item.Quantity += quantity;
                return;
            }

            _items.Add(new ConsumableInfo(consumable, quantity));
        }

        public void Remove(ConsumableSO consumable, int quantity = 1)
        {
            if (quantity <= 0) return;

            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                if (item.Data != consumable) continue;
                item.Quantity -= quantity;
                if (item.Quantity <= 0) _items.RemoveAt(index);

                return;
            }
        }

        public bool Contains(ConsumableSO consumable)
        {
            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                if (item.Data == consumable) return true;
            }

            return false;
        }
    }
}