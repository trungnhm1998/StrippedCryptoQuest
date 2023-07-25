using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Home
{
    public class UIHomeMenuSortCharacter : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;

        private UICharacterCard _selectedCardHolder;

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value % _partyManagerMockData.Members.Count;

                if (_currentIndex < 0)
                    _currentIndex = _partyManagerMockData.Members.Count - 1;
            }
        }

        private void Awake()
        {
            _inputMediator.EnableMenuInput();
        }

        private void OnEnable()
        {
            _inputMediator.HomeMenuSortEvent += OnEnableSortFunc;
            RegisterSelectInputEvents();
        }

        private void OnDisable()
        {
            _inputMediator.HomeMenuSortEvent -= OnEnableSortFunc;
            UnregisterSelectInputEvent();
        }

        private UICharacterCard GetCharacterCard(int index)
        {
            var cardUI = _characterSlots.GetChild(index).GetComponent<UICharacterCard>();
            return cardUI;
        }

        private void OnEnableSortFunc()
        {
            _inputMediator.EnableHomeMenuInput();
            _selectedCardHolder = GetCharacterCard(0);
            SelectTargetToSort();
        }

        private void OnNextTarget()
        {
            CurrentIndex++;
            SelectTargetToSort();
        }

        private void OnPreviousTarget()
        {
            CurrentIndex--;
            SelectTargetToSort();
        }

        private void SelectTargetToSort()
        {
            var currentCard = GetCharacterCard(CurrentIndex);

            if (_selectedCardHolder != currentCard)
                _selectedCardHolder.Deselect();

            _selectedCardHolder = currentCard;
            _selectedCardHolder.Select();
        }

        private void OnConfirmSelect()
        {
            UnregisterSelectInputEvent();
            RegisterSortInputEvents();

            _topLine.SetActive(false);
            _selectedCardHolder.OnSelected();
        }

        private void OnSwapRight()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex++;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardHolder = currentTarget.GetComponent<UICharacterCard>();
        }

        private void OnSwapLeft()
        {

            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex--;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardHolder = currentTarget.GetComponent<UICharacterCard>();
        }

        private void OnConfirmSortOrder()
        {
            _selectedCardHolder.Confirm();
            _topLine.SetActive(true);
            UnregisterSortInputEvents();
            RegisterSelectInputEvents();
        }

        private void RegisterSelectInputEvents()
        {
            _inputMediator.NextEvent += OnNextTarget;
            _inputMediator.PreviousEvent += OnPreviousTarget;
            _inputMediator.ConfirmEvent += OnConfirmSelect;
        }
        
        private void UnregisterSelectInputEvent()
        {
            _inputMediator.NextEvent -= OnNextTarget;
            _inputMediator.PreviousEvent -= OnPreviousTarget;
            _inputMediator.ConfirmEvent -= OnConfirmSelect;
        }

        private void RegisterSortInputEvents()
        {
            _inputMediator.NextEvent += OnSwapRight;
            _inputMediator.PreviousEvent += OnSwapLeft;
            _inputMediator.ConfirmEvent += OnConfirmSortOrder;
        }

        private void UnregisterSortInputEvents()
        {
            _inputMediator.NextEvent -= OnSwapRight;
            _inputMediator.PreviousEvent -= OnSwapLeft;
            _inputMediator.ConfirmEvent -= OnConfirmSortOrder;
        }
    }
}
