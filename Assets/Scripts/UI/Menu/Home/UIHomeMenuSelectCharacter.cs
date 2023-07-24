using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Home
{
    public class UIHomeMenuSelectCharacter : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        private UICharacterCard _selectingCardHolder;
        private UICharacterCard _selectedCardHolder;
        private int[] _arr = { 0, 0, 0, 0 };

        private int _currentSortTargetIndex = 0;
        private int CurrentSortTargetIndex
        {
            get => _currentSortTargetIndex;
            set
            {
                _currentSortTargetIndex = value % _partyManagerMockData.Members.Count;

                if (_currentSortTargetIndex < 0)
                    _currentSortTargetIndex = _partyManagerMockData.Members.Count - 1;
            }
        }

        private void Awake()
        {
            _inputMediator.EnableMenuInput();
        }

        private void OnEnable()
        {
            _inputMediator.HomeMenuSortEvent += OnEnableSortFunc;
            _inputMediator.NextEvent += OnNextTarget;
            _inputMediator.PreviousEvent += OnPreviousTarget;
            _inputMediator.ConfirmSelectEvent += OnConfirmSelectTarget;
        }

        private void OnDisable()
        {
            _inputMediator.HomeMenuSortEvent -= OnEnableSortFunc;
            _inputMediator.NextEvent -= OnNextTarget;
            _inputMediator.PreviousEvent -= OnPreviousTarget;
            _inputMediator.ConfirmSelectEvent -= OnConfirmSelectTarget;
        }

        private UICharacterCard GetCharacterCard(int index)
        {
            var cardUI = _characterSlots.GetChild(index).GetComponent<UICharacterCard>();
            return cardUI;
        }

        private void OnEnableSortFunc()
        {
            _inputMediator.EnableHomeMenuInput();
            _selectingCardHolder = GetCharacterCard(0);
            SelectTargetToSort();
        }

        private void OnNextTarget()
        {
            CurrentSortTargetIndex++;
            SelectTargetToSort();
        }

        private void OnPreviousTarget()
        {
            CurrentSortTargetIndex--;
            SelectTargetToSort();
        }

        private void SelectTargetToSort()
        {
            var currentCard = GetCharacterCard(CurrentSortTargetIndex);

            if (_selectingCardHolder != currentCard)
                _selectingCardHolder.Deselect();

            _selectingCardHolder = currentCard;
            _selectingCardHolder.Select();
        }

        private void OnConfirmSelectTarget()
        {
            // _selectedCardHolder = _selectingCardHolder;
            _arr[CurrentSortTargetIndex] = 1;

            Debug.Log($"[{string.Join(",", _arr)}]");
            _inputMediator.NextEvent -= OnNextTarget;
            _inputMediator.PreviousEvent -= OnPreviousTarget;

            _selectingCardHolder.OnSelected();

            _inputMediator.NextEvent += OnSwapRight;
        }

        private void OnSwapRight()
        {

        }
    }
}
