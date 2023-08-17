using System.Collections.Generic;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    // TODO: refactor getChild() and getComponent() code style
    public class UIHomeMenuSortCharacter : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PartySO _party;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO SortModeEnabledEvent;
        [SerializeField] private VoidEventChannelSO SortModeDisabledEvent;

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;
        [SerializeField] private List<UICharacterInfo> _slots;

        private UICharacterCard _selectedCardHolder;

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                // int count = _party.Members.Count;
                // _currentIndex = (value + count) % count;
            }
        }

        private int _indexHolder;

        private void Awake()
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            // for (int i = 0; i < _party.Members.Count; i++)
            // {
            //     _slots[i].SetData(_party.Members[i]);
            // }
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

        private void OnEnableSortMode()
        {
            SortModeEnabledEvent.RaiseEvent();
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
            // for (int i = 0; i < _party.Members.Count; i++)
            // {
            //     var memberInfo = _characterSlots.GetChild(i).GetComponent<UICharacterInfo>().CharInfoMockData;
            //     _party.Members[i] = memberInfo;
            // }
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
            SortModeDisabledEvent.RaiseEvent();
            _selectedCardHolder.Deselect();
            UnregisterSortModeEvent();
            UnregisterSelectInputEvent();
        }

        private void RegisterSortModeEvent()
        {
            _inputMediator.HomeMenuSortEvent += OnEnableSortMode;
        }

        private void UnregisterSortModeEvent()
        {
            _inputMediator.HomeMenuSortEvent -= OnEnableSortMode;
        }

        private void RegisterSelectInputEvents()
        {

        }

        private void UnregisterSelectInputEvent()
        {

        }

        private void RegisterSortInputEvents()
        {

        }

        private void UnregisterSortInputEvents()
        {

        }
    }
}