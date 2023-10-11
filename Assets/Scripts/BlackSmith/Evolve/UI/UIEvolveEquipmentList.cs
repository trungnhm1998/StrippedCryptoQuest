using System;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _itemPrefab;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _finishedRenderEvent;

        private List<UIEquipmentItem> _equipmentList = new();
        public List<UIEquipmentItem> EquipmentList { get => _equipmentList; }

        public void RenderEquipments(List<IEvolvableData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                InstantiateNewEquipmentUI(data[i]);
            }

            SelectDefaultButton();
            _finishedRenderEvent.Invoke();
        }

        private void InstantiateNewEquipmentUI(IEvolvableData equipmentData)
        {
            var go = Instantiate(_itemPrefab, _scrollRect.content);
            var equipmentUi = go.GetComponent<UIEquipmentItem>();
            equipmentUi.SetItemData(equipmentData);

            _equipmentList.Add(equipmentUi);
        }

        private void SelectDefaultButton()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }
    }
}