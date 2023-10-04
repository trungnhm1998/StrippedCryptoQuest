using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade.MVP
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<InventorySO> _instantiateData;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _instantiateData.Invoke(inventory);
        }
    }
}