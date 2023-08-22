using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    // TODO: refactor getChild() and getComponent() code style
    public class UIHomeMenuSortCharacter : MonoBehaviour
    {
        public event Action SelectedEvent;
        public event Action ConfirmedEvent;

        [Header("Configs")]
        [SerializeField] private PartySO _party;
        [SerializeField] private UIHomeMenu _homeMenu;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO SortModeEnabledEvent;

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;
        [SerializeField] private List<UICharacterInfo> _slots;
        [SerializeField] private List<UICharacterCardButton> _cardButtons;
        [SerializeField] private GameObject _sortKeysUi;

        private UICharacterCardButton _selectedCardButtonHolder;
        private List<HeroDataSO> _activeMembersData = new();
        private List<AttributeSystemBehaviour> _activeMembersAttribute = new();

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _activeMembersData.Count;
                _currentIndex = (value + count) % count;
            }
        }

        private int _indexHolder;
        private HeroDataSO _memberStats;

        private void OnEnable()
        {
            _homeMenu.OpenedEvent += LoadPartyMembers;
            UICharacterCardButton.SelectedEvent += ConfirmSelect;
        }

        private void OnDisable()
        {
            _homeMenu.OpenedEvent -= LoadPartyMembers;
            UICharacterCardButton.SelectedEvent -= ConfirmSelect;
        }

        private void LoadPartyMembers()
        {
            var i = 0;
            foreach (var member in _party.PlayerTeam.Members)
            {
                if (member.gameObject.activeSelf)
                {
                    Debug.Log($"member = [{member.name}]");
                    member.TryGetComponent<StatsInitializer>(out var initializer);
                    _memberStats = initializer.DefaultStats as HeroDataSO;
                    _activeMembersData.Add(_memberStats);
                    _activeMembersAttribute.Add(member.AttributeSystem);
                    _slots[i].SetData(_memberStats, _activeMembersAttribute[i]);
                    i++;
                }
            }
        }

        private UICharacterCardButton GetCharacterCard(int index)
        {
            var cardUI = _characterSlots.GetChild(index).GetComponent<UICharacterCardButton>();
            return cardUI;
        }

        private void OnEnableSortMode()
        {
            SortModeEnabledEvent.RaiseEvent();
            _selectedCardButtonHolder = GetCharacterCard(CurrentIndex);
            _selectedCardButtonHolder.Select();
        }

        public void ConfirmSelect(UICharacterCardButton card)
        {
            SelectedEvent?.Invoke();

            _topLine.SetActive(false);
            _sortKeysUi.SetActive(true);

            CurrentIndex = card.transform.GetSiblingIndex();
            _indexHolder = CurrentIndex;
            _selectedCardButtonHolder = card;
        }

        public void SwapRight()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            _party.Sort(CurrentIndex, CurrentIndex + 1);
            CurrentIndex++;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void SwapLeft()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            _party.Sort(CurrentIndex, CurrentIndex - 1);
            CurrentIndex--;
            currentTarget.SetSiblingIndex(CurrentIndex);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void ConfirmSortOrder()
        {
            _selectedCardButtonHolder.Confirm();
            _topLine.SetActive(true);

            Invoke(nameof(OnConfirmSortOrder), 0.5f);
        }

        public void OnConfirmSortOrder()
        {
            ConfirmedEvent?.Invoke();
        }

        public void CancelSort()
        {
            _selectedCardButtonHolder.Cancel();
            ApplyDataBeforeSort();
        }

        private void ApplyDataBeforeSort()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);
            currentTarget.SetSiblingIndex(_indexHolder);

            CurrentIndex = _indexHolder;
        }

        private void EnableButtons()
        {
            foreach (var button in _cardButtons)
            {
                button.enabled = true;
            }
        }

        private void DisableButtons()
        {
            foreach (var button in _cardButtons)
            {
                button.enabled = false;
            }
        }

        public void Init()
        {
            OnEnableSortMode();
            EnableButtons();
            _sortKeysUi.SetActive(true);
        }

        public void DeInit()
        {
            DisableButtons();
            _sortKeysUi.SetActive(false);
        }
    }
}