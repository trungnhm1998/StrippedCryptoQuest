using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class MockConsumableProvider : MonoBehaviour, IConsumablesProvider
    {
        [SerializeField] private List<UsableInfo> _items = new();
        public List<UsableInfo> Items => _items;
    }
}