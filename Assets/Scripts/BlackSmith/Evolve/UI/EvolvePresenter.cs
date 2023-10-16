using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private UIEvolveEquipmentList _evolvableEquipmentListUi;
        [SerializeField] private LocalizedString _message;
        [SerializeField] private LocalizedString _message1;
        [SerializeField] private BlackSmithDialogManager _dialogManager;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent<List<IEvolvableData>> _getInventoryEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _viewEquipmentDetailEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();

        private void OnEnable()
        {
            StartCoroutine(GetEquipments());
            _dialogManager.Dialog.SetMessage(_message);
        }

        private void OnDisable()
        {
            StopCoroutine(GetEquipments());
            UnregisterEquipmentEvent();
        }

        private IEnumerator GetEquipments()
        {
            _equipmentModel = GetComponentInChildren<IEvolvableEquipment>();
            yield return _equipmentModel.CoGetData();

            Debug.Log($"_equipmentModel.EvolvableData = {_equipmentModel.EvolvableData.Count}");

            _gameData = _equipmentModel.EvolvableData;

            Debug.Log($"_gameData = {_gameData.Count}");
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
            _dialogManager.Dialog.SetMessage(_message1);
        }
    }
}