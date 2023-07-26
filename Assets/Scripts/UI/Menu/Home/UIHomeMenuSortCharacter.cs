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
        [SerializeField] private VoidEventChannelSO _enableMainMenuInputs;
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;

        private UICharacterCard _selectedCardHolder;

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _partyManagerMockData.Members.Count;
                _currentIndex = (value + count) % count;
            }
        }

        private int _indexHolder;

        private void Awake()
        {
            _inputMediator.EnableMenuInput();
        }

        private void OnEnable()
        {
            RegisterSortModeEvent();
            RegisterSelectInputEvents();
        }

        private void OnDisable()
        {
            UnregisterSortModeEvent();
            UnregisterSelectInputEvent();
        }

        private UICharacterCard GetCharacterCard(int index)
        {
            var cardUI = _characterSlots.GetChild(index).GetComponent<UICharacterCard>();
            return cardUI;
        }

        private void EnableSortMode()
        {
            _inputMediator.EnableHomeMenuInput();
            _selectedCardHolder = GetCharacterCard(0);
            SelectTargetToSort();
        }

        private void ChangeNextTarget()
        {
            CurrentIndex++;
            SelectTargetToSort();
        }

        private void ChangePreviousTarget()
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

        private void ConfirmSelect()
        {
            UnregisterSelectInputEvent();
            RegisterSortInputEvents();

            _topLine.SetActive(false);
            _selectedCardHolder.Selected();

            _indexHolder = CurrentIndex;
        }

        private void SwapRight()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex++;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardHolder = currentTarget.GetComponent<UICharacterCard>();
        }

        private void SwapLeft()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex--;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardHolder = currentTarget.GetComponent<UICharacterCard>();
        }

        private void ConfirmSortOrder()
        {
            _selectedCardHolder.Confirm();
            _topLine.SetActive(true);
            MatchDataWithUI();
            UnregisterSortInputEvents();
            RegisterSelectInputEvents();
        }

        private void MatchDataWithUI()
        {
            for (int i = 0; i < _partyManagerMockData.Members.Count; i++)
            {
                var memberInfo = _characterSlots.GetChild(i).GetComponent<UICharacterInfo>().CharInfoMockData;
                _partyManagerMockData.Members[i] = memberInfo;
            }
        }

        private void CancelSort()
        {
            _selectedCardHolder.Cancel();
            UnregisterSortInputEvents();
            RegisterSelectInputEvents();
            ApplyDataBeforeSort();
        }

        private void ApplyDataBeforeSort()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);
            currentTarget.SetSiblingIndex(_indexHolder);

            CurrentIndex = _indexHolder;
        }
        
        private void CancelSelect()
        {
            _selectedCardHolder.Deselect();
            _enableMainMenuInputs.RaiseEvent();
        }

        private void RegisterSortModeEvent()
        {
            _inputMediator.HomeMenuSortEvent += EnableSortMode;
        }
        
        private void UnregisterSortModeEvent()
        {
            _inputMediator.HomeMenuSortEvent -= EnableSortMode;
        }

        private void RegisterSelectInputEvents()
        {
            _inputMediator.NextEvent += ChangeNextTarget;
            _inputMediator.PreviousEvent += ChangePreviousTarget;
            _inputMediator.ConfirmEvent += ConfirmSelect;
            _inputMediator.HomeMenuCancelEvent += CancelSelect;
        }
        
        private void UnregisterSelectInputEvent()
        {
            _inputMediator.NextEvent -= ChangeNextTarget;
            _inputMediator.PreviousEvent -= ChangePreviousTarget;
            _inputMediator.ConfirmEvent -= ConfirmSelect;
            _inputMediator.HomeMenuCancelEvent -= CancelSelect;
        }

        private void RegisterSortInputEvents()
        {
            _inputMediator.NextEvent += SwapRight;
            _inputMediator.PreviousEvent += SwapLeft;
            _inputMediator.ConfirmEvent += ConfirmSortOrder;
            _inputMediator.HomeMenuCancelEvent += CancelSort;
        }

        private void UnregisterSortInputEvents()
        {
            _inputMediator.NextEvent -= SwapRight;
            _inputMediator.PreviousEvent -= SwapLeft;
            _inputMediator.ConfirmEvent -= ConfirmSortOrder;
            _inputMediator.HomeMenuCancelEvent -= CancelSort;
        }
    }
}
