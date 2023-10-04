using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade.MVP
{
    public class UIUpgradeEquipment : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _equipmentPrefab;
        public void InstantiateData (InventorySO inventory)
        {
            CleanUpScrollView();
            var listItem = inventory.Equipments;
            foreach (var item in listItem)
            {
                IUpgradeEquipment equipmentData = new UpgradeEquipment(item);
                var equipment = Instantiate(_equipmentPrefab, _scrollRect.content).GetComponent<UIUpgradeItem>();
                equipment.ConfigureCell(equipmentData);
            }
            SelectDefaultButton();
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
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
    }
}