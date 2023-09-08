using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UIHomeMenuSortCharacter : MonoBehaviour
    {
        public event Action SelectedEvent;
        public event Action ConfirmedEvent;
        [SerializeField] private ServiceProvider _serviceProvider;

        [Header("Configs")]
        [SerializeField] private UIHomeMenu _homeMenu;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO SortModeEnabledEvent;

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;
        [SerializeField] private UIPartySlot[] _partySlots;
        [SerializeField] private List<ICharacterInfo> _slots;
        [SerializeField] private List<UICharacterCardButton> _cardButtons;
        [SerializeField] private GameObject _sortKeysUi;

        private UICharacterCardButton _selectedCardButtonHolder;

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _partySlots.Length;
                _currentIndex = (value + count) % count;
            }
        }

        private int _indexHolder;
        private HeroData _memberStats;
        private IParty _party;

        private void OnValidate()
        {
            if (_partySlots.Length != PartyConstants.MAX_PARTY_SIZE)
            {
                Array.Resize(ref _partySlots, PartyConstants.MAX_PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<UIPartySlot>();

            Debug.Log($"UIHomeMenuSortCharacter::OnValidate()");
        }

        private void Awake()
        {
            _party = _serviceProvider.PartyController.Party;
            _serviceProvider.PartyProvided += InitParty;
        }

        private void OnEnable()
        {
            UICharacterCardButton.SelectedEvent += ConfirmSelect;
            _sortKeysUi.SetActive(true);
            LoadPartyMembers();
        }

        private void OnDisable()
        {
            UICharacterCardButton.SelectedEvent -= ConfirmSelect;
        }

        private void OnDestroy()
        {
            _serviceProvider.PartyProvided -= InitParty;
        }

        private void InitParty(IPartyController partyController)
        {
            _party = _serviceProvider.PartyController.Party;
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            for (var index = 0; index < _party.Members.Length; index++)
            {
                var member = _party.Members[index];
                var slot = _partySlots[index];
                slot.Active(member.IsValid());
                if (!member.IsValid()) continue;
                slot.Init(member);
            }
        }

        private UICharacterCardButton GetCharacterCard(int index)
        {
            var cardUI = _cardButtons[index];
            return cardUI;
        }

        private void OnEnableSortMode()
        {
            SortModeEnabledEvent.RaiseEvent();

            Debug.Log($"current index = [{CurrentIndex}]");

            _selectedCardButtonHolder = GetCharacterCard(CurrentIndex);
            Debug.Log($"current card = [{_selectedCardButtonHolder}]");
            _selectedCardButtonHolder.Select();
        }

        public void ConfirmSelect(UICharacterCardButton card)
        {
            SelectedEvent?.Invoke();

            _topLine.SetActive(false);
            _sortKeysUi.SetActive(true);

            CurrentIndex = card.transform.GetSiblingIndex();
            _indexHolder = CurrentIndex;
            // _selectedCardButtonHolder = card;
        }

        public void SwapRight()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex++;
            currentTarget.SetSiblingIndex(CurrentIndex);

            // _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void SwapLeft()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);

            CurrentIndex--;
            currentTarget.SetSiblingIndex(CurrentIndex);

            // _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void ConfirmSortOrder()
        {
            // _selectedCardButtonHolder.Confirm();
            _topLine.SetActive(true);

            Invoke(nameof(OnConfirmSortOrder), 0.5f);
        }

        public void OnConfirmSortOrder()
        {
            ConfirmedEvent?.Invoke();
        }

        public void CancelSort()
        {
            // _selectedCardButtonHolder.Cancel();
            ApplyDataBeforeSort();
        }

        private void ApplyDataBeforeSort()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);
            currentTarget.SetSiblingIndex(_indexHolder);

            CurrentIndex = _indexHolder;
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
        }

        public void DeInit()
        {
            DisableButtons();
            _sortKeysUi.SetActive(false);
        }
    }
}