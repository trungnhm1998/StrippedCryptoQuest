using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class EquipmentSectionPresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<List<IData>> SetDataEvent;
        [SerializeField] private Transform _gameNftList;
        [SerializeField] private Transform _wallet;

        private IEquipmentModel _model;

        private void Awake()
        {
            UITransferItem.SelectItemEvent += ItemSelected;
        }

        private void OnDestroy()
        {
            UITransferItem.SelectItemEvent -= ItemSelected;
        }

        private IEnumerator Start()
        {
            _model = GetComponent<IEquipmentModel>();
            yield return _model.CoGetData();
            var data = _model.Data;
            SetDataEvent.Invoke(data);
        }

        private void ItemSelected(UITransferItem currentItem)
        {
            currentItem.Transfer(_wallet);
        }
    }
}
