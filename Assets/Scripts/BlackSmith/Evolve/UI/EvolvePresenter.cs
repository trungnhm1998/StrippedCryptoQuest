using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private BlackSmithDialogManager _dialogManager;

        [SerializeField] private UIEvolveEquipmentList _evolvableEquipmentListUi;
        [SerializeField] private UIEquipmentDetail _equipmentDetailUi;
        [SerializeField] private UICharactersPreview _characterPreviewUi;
        [SerializeField] private UIConfirmPanel _confirmPanel;
        [SerializeField] private UIResultPanel _resultPanel;

        [SerializeField] private LocalizedString _selectTargetMessage;
        [SerializeField] private LocalizedString _selectMaterialMessage;
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _evolveSuccessMessage;
        [SerializeField] private LocalizedString _evolveFailedMessage;


        [Header("Unity Events")]
        [SerializeField] private UnityEvent<List<IEvolvableData>> _getInventoryEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _viewEquipmentDetailEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _evolveSuccessEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _evolveFailedEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();

        private UIEquipmentItem _selectedEquipment;

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
            _selectedEquipment = selectedEquipment;
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

        public void ProceedEvolve()
        {
            _confirmPanel.gameObject.SetActive(false);
            CheckEvolveResult();
        }

        public void CancelEvolve()
        {
            _confirmPanel.gameObject.SetActive(false);
        }

        private void CheckEvolveResult()
        {
            _resultPanel.gameObject.SetActive(true);
            _evolvableEquipmentListUi.gameObject.SetActive(false);
            _equipmentDetailUi.gameObject.SetActive(false);
            _characterPreviewUi.gameObject.SetActive(false);

            Random rand = new Random();
            int rd = rand.Next(100);
            bool isSuccess = rd < _selectedEquipment.EquipmentData.Rate;

            Debug.Log($"Evolve Rate = {rd}");

            if (isSuccess)
            {
                _evolveSuccessEvent.Invoke(_selectedEquipment.EquipmentData);
                _dialogManager.Dialogue.SetMessage(_evolveSuccessMessage);
            }
            else
            {
                _evolveFailedEvent.Invoke(_selectedEquipment.EquipmentData);
                _dialogManager.Dialogue.SetMessage(_evolveFailedMessage);
            }

            _dialogManager.Dialogue.Show();
        }
    }
}