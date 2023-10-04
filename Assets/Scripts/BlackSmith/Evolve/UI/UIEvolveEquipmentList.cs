using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _itemPrefab;

        public void RenderEquipments(InventorySO inventory)
        {
            for (int i = 0; i < inventory.Equipments.Count; i++)
            {
                var equipment = inventory.Equipments[i];
                InstantiateNewEquipmentUI(equipment);
            }

            SelectDefaultButton();
        }

        private void InstantiateNewEquipmentUI(EquipmentInfo equipment)
        {
            var go = Instantiate(_itemPrefab, _scrollRect.content);
            go.GetComponent<UIEquipmentItem>().SetItemData(equipment);
        }

        private void SelectDefaultButton()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }
    }
}