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

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradeEquipment _upgradeEquipment;
        [SerializeField] private UIUpgradeResult _result;
        private IUpgradeEquipment _equipmentData;

        private void OnEnable()
        {
            Init();
            UIUpgradeItem.SelectedItemEvent += OnSelecetedEquipment;
            // _input.NavigateEvent += SelectLevelToUpgrade; Because remove old input action to resolved conflict
        }

        private void OnDisable()
        {
            UIUpgradeItem.SelectedItemEvent -= OnSelecetedEquipment;
            // _input.NavigateEvent -= SelectLevelToUpgrade; Because remove old input action to resolved conflict
        }

        public void Init()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _upgradeEquipment.InstantiateData(inventory);
        }

        private void OnSelecetedEquipment(UIUpgradeItem item)
        {
            _upgradeEquipment.SelectedEquipment(item);
            _equipmentData = item.Equipment;
        }

        public void UpgradeEquipment()
        {
            _upgradeEquipment.SetLevel(_equipmentData);
            _result.RenderEquipment(_equipmentData);
        }

        private void SelectLevelToUpgrade(Vector2 direction)
        {
            if (direction.y > 0)    
                _upgradeEquipment.SetValue(1, _equipmentData);
            if (direction.y < 0)
                _upgradeEquipment.SetValue(-1, _equipmentData);
        }
    }
}