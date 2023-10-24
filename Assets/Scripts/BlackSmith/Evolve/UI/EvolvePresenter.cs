using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.EvolveStates;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        public event UnityAction WaitForDataAvailableEvent;
        public event UnityAction EnterConfirmPhaseEvent;
        public event UnityAction ExitConfirmPhaseEvent;

        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private EvolveStateController _evolveController;

        [SerializeField] private UIEvolveEquipmentList _evolvableEquipmentListUi;
        [SerializeField] private UIEquipmentDetail _equipmentDetailUi;
        [SerializeField] private UICharactersPreview _characterPreviewUi;
        [SerializeField] private UIConfirmPanel _confirmPanel;
        [SerializeField] private UIResultPanel _resultPanel;

        [SerializeField] private LocalizedString _selectMaterialMessage;
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _evolveSuccessMessage;
        [SerializeField] private LocalizedString _evolveFailedMessage;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent<IEvolvableData> _viewEquipmentDetailEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _evolveSuccessEvent;
        [SerializeField] private UnityEvent<IEvolvableData> _evolveFailedEvent;

        private IEvolvableEquipment _equipmentModel;
        private List<IEvolvableData> _gameData = new();
        public List<IEvolvableData> GameData { get => _gameData; }

        private UIEquipmentItem _selectedEquipment;

        /// <summary>
        /// Somehow the MaterialSelected method run 3 times after canceling the YesNoDialog/pressing Esc to turn back so this is the temp solution. Might refactor later.
        /// </summary>
        public bool HadMethodRunned { get; set; }

        private void OnEnable()
        {
            _dialogManager.ConfirmYesEvent += ProceedEvolve;
            _dialogManager.ConfirmNoEvent += CancelEvolve;
            _evolveController.ExitConfirmPhaseEvent += ShowEvolveEquipment;

            StartCoroutine(GetEquipment());
        }

        private void OnDisable()
        {
            _dialogManager.ConfirmYesEvent -= ProceedEvolve;
            _dialogManager.ConfirmNoEvent -= CancelEvolve;
            _evolveController.ExitConfirmPhaseEvent -= ShowEvolveEquipment;

            StopCoroutine(GetEquipment());
            UnregisterEquipmentEvent();
        }

        public IEnumerator GetEquipment()
        {
            _equipmentModel = GetComponentInChildren<IEvolvableEquipment>();
            yield return _equipmentModel.CoGetData();
            SaveEquipmentDataInterval();
        }

        private void SaveEquipmentDataInterval()
        {
            while (_gameData.Count <= 0)
            {
                _gameData = _equipmentModel.EvolvableData;

                Debug.Log($"_gameData = {_gameData.Count}");
            }
            WaitForDataAvailableEvent?.Invoke();
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
            if (HadMethodRunned) return;
            HadMethodRunned = true;

            EnterConfirmPhaseEvent?.Invoke();
            _dialogManager.Dialogue.Hide();
            _dialogManager.ShowConfirmDialog(_confirmMessage);
        }

        private void ProceedEvolve()
        {
            _confirmPanel.gameObject.SetActive(false);
            CheckEvolveResult();
        }

        private void CancelEvolve()
        {
            _confirmPanel.gameObject.SetActive(false);
            ExitConfirmPhaseEvent?.Invoke();
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

        private void ShowEvolveEquipment()
        {
            _resultPanel.gameObject.SetActive(false);
            _evolvableEquipmentListUi.gameObject.SetActive(true);
            _equipmentDetailUi.gameObject.SetActive(true);
            _characterPreviewUi.gameObject.SetActive(true);            
        }
    }
}