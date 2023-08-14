using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class EquipmentFilters : MonoBehaviour
    {
        [SerializeField] private EquipmentTypeSO[] filters;

        private Dictionary<EquipmentTypeSO, int> _cached;

        private Dictionary<EquipmentTypeSO, int> Cached
        {
            get
            {
                LazyInit();
                return _cached;
            }
        }

        public bool SameType(EquipmentTypeSO targetEquipment)
        {
            return Cached.ContainsKey(targetEquipment);
        }

        private void LazyInit()
        {
            if (_cached != null) return;
            _cached = new();
            var index = 0;
            for (int i = 0; i < filters.Length; i++)
            {
                _cached[filters[i]] = index;
            }
        }
    }
}