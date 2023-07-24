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
        [SerializeField] private UICharacterCard _cardHolder;

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
            _inputMediator.NextTargetEvent += OnNextTarget;
            _inputMediator.PreviousTargetEvent += OnPreviousTarget;
        }

        private void OnDisable()
        {
            _inputMediator.HomeMenuSortEvent -= OnEnableSortFunc;
            _inputMediator.NextTargetEvent -= OnNextTarget;
            _inputMediator.PreviousTargetEvent += OnPreviousTarget;
        }

        private UICharacterCard GetCharacterCard(int index)
        {
            var cardUI = _characterSlots.GetChild(index).GetComponent<UICharacterCard>();
            return cardUI;
        }

        private void OnEnableSortFunc()
        {
            _inputMediator.EnableHomeMenuInput();
            _cardHolder = GetCharacterCard(0);
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

            if (_cardHolder != currentCard)
                _cardHolder.Deselect();

            _cardHolder = currentCard;
            _cardHolder.Select();
        }
    }
}
