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
        [SerializeField] private BlackSmithDialogManager _dialogManager;
        [SerializeField] private UIEvolveEquipmentList _evolvableEquipmentListUi;
        [SerializeField] private UIConfirmPanel _confirmPanel;
        [SerializeField] private LocalizedString _selectTargetMessage;
        [SerializeField] private LocalizedString _selectMaterialMessage;
        [SerializeField] private LocalizedString _confirmMessage;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent<List<IEvolvableData>> _getInventoryEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _viewEquipmentDetailEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();

        private void OnEnable()
        {
            StartCoroutine(GetEquipment());
            _dialogManager.Dialogue.SetMessage(_selectTargetMessage);
        }

        private void OnDisable()
        {
            StopCoroutine(GetEquipment());
            UnregisterEquipmentEvent();
        }

        private IEnumerator GetEquipment()
        {
            _equipmentModel = GetComponentInChildren<IEvolvableEquipment>();
            yield return _equipmentModel.CoGetData();
            SetEquipmentDataInterval();
        }

        private void SetEquipmentDataInterval()
        {
            while (_gameData.Count <= 0)
            {
                _gameData = _equipmentModel.EvolvableData;

                Debug.Log($"_gameData = {_gameData.Count}");
                _getInventoryEvent.Invoke(_gameData);
            }
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
                item.SelectedEquipmentAsMaterialEvent += MaterialSelected;
            }
        }

        private void UnregisterEquipmentEvent()
        {
            foreach (var item in _evolvableEquipmentListUi.EquipmentList)
            {
                item.InspectingEquipmentEvent -= InspectingEquipment;
                item.SelectedEquipmentEvent -= EquipmentSelected;
                item.SelectedEquipmentAsMaterialEvent -= MaterialSelected;
            }
        }

        private void InspectingEquipment(IEvolvableData equipmentData)
        {
            _viewEquipmentDetailEvent.Invoke(equipmentData);
        }

        private void EquipmentSelected(UIEquipmentItem selectedEquipment)
        {
            foreach (var item in _evolvableEquipmentListUi.EquipmentList)
            {
                HideEquipmentThatIsNotMaterial(item, selectedEquipment);
                NotifyToOtherEquipmentThatTheTargetWasPicked(item, selectedEquipment);
            }
            _dialogManager.Dialogue.SetMessage(_selectMaterialMessage);
        }

        private void HideEquipmentThatIsNotMaterial(UIEquipmentItem itemInList, UIEquipmentItem itemSelected)
        {
            if (itemInList.EquipmentData != itemSelected.EquipmentData)
                itemInList.gameObject.SetActive(false);
        }

        private void NotifyToOtherEquipmentThatTheTargetWasPicked(UIEquipmentItem itemInList, UIEquipmentItem itemSelected)
        {
            if (itemInList.EquipmentData == itemSelected.EquipmentData)
                itemInList.IsTargetSelected = true;
        }

        private void MaterialSelected(UIEquipmentItem selectedMaterial)
        {
            _confirmPanel.gameObject.SetActive(true);
            _dialogManager.Dialogue.Hide();
            _dialogManager.ShowConfirmDialog(_confirmMessage);
        }
    }
}