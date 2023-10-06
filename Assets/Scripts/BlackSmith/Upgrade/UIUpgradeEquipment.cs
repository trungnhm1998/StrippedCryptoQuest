using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeEquipment : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _equipmentPrefab;
        [SerializeField] private RectTransform _upgradeEquipmentPanel;
        [SerializeField] private Transform _selectedEquipmentPanel;
        [SerializeField] private TextMeshProUGUI _levelToUpgrade;
        private int _upgradeLevel;

        public void SetValue(int level, IUpgradeEquipment item)
        {
            int currentLevel = _upgradeLevel;
            currentLevel += level;

            if (currentLevel < item.Level) return;
            _upgradeLevel = currentLevel;
            _levelToUpgrade.text = _upgradeLevel.ToString();
        }

        public void InstantiateData(InventorySO inventory)
        {
            CleanUpScrollView();
            var listItem = inventory.Equipments;
            foreach (var item in listItem)
            {
                IUpgradeEquipment equipmentData = new UpgradeEquipment(item);
                var equipment = Instantiate(_equipmentPrefab, _scrollRect.content).GetComponent<UIUpgradeItem>();
                equipment.ConfigureCell(equipmentData);
            }
            StartCoroutine(SelectDefaultButton());
        }

        public void SetLevel(IUpgradeEquipment item)
        {
            item.Equipment.SetLevel(_upgradeLevel);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in _selectedEquipmentPanel)
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            if (_scrollRect.content.childCount == 0) yield break;
            var firstItemGO = _scrollRect.content.GetChild(0).gameObject;
            EventSystem.current.SetSelectedGameObject(firstItemGO);
        }

        public void SelectedEquipment(UIUpgradeItem item)
        {
            item.transform.SetParent(_upgradeEquipmentPanel);
            _levelToUpgrade.text = item.Equipment.Level.ToString();
        }
    }
}