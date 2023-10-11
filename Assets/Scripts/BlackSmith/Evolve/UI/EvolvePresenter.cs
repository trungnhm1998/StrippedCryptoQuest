using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private UIEvolveEquipmentList _evolvableEquipmentListUi;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent<List<IEvolvableData>> _getInventoryEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _viewEquipmentDetailEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();

        private void Start()
        {
            StartCoroutine(GetEquipments());
        }

        private void OnDisable()
        {
            UnregisterEquipmentEvent();
        }

        private IEnumerator GetEquipments()
        {
            _equipmentModel = GetComponentInChildren<IEvolvableEquipment>();
            yield return _equipmentModel.CoGetData();

            if (_equipmentModel.EvolvableData.Count <= 0) yield break;

            _gameData = _equipmentModel.EvolvableData;
            _getInventoryEvent.Invoke(_gameData);
        }

        public void FinishedRenderEquipmentList()
        {
            RegisterEquipmentEvent();
        }

        private void RegisterEquipmentEvent()
        {
            foreach (var item in _evolvableEquipmentListUi.EquipmentList)
            {
                item.InspectingEquipmentEvent += InspectingEquipment;
                item.SelectedEquipmentEvent += EquipmentSelected;
            }
        }

        private void UnregisterEquipmentEvent()
        {
            foreach (var item in _evolvableEquipmentListUi.EquipmentList)
            {
                item.InspectingEquipmentEvent -= InspectingEquipment;
                item.SelectedEquipmentEvent -= EquipmentSelected;
            }
        }

        private void InspectingEquipment(IEvolvableData equipmentData)
        {
            _viewEquipmentDetailEvent.Invoke(equipmentData);
        }

        private void EquipmentSelected(UIEquipmentItem singleEquipmentUi)
        {
            foreach (var item in _evolvableEquipmentListUi.EquipmentList)
            {
                if (item.EquipmentData != singleEquipmentUi.EquipmentData)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}