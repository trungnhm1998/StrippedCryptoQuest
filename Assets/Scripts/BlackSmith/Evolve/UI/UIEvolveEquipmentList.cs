using System;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _itemPrefab;

        public void RenderEquipments(List<IEvolvableData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                InstantiateNewEquipmentUI(data[i]);
            }

            SelectDefaultButton();
        }

        private void InstantiateNewEquipmentUI(IEvolvableData equipment)
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