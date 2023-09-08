using System;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Game Components")]
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _topLine;
        [SerializeField] private UIPartySlot[] _partySlots;
        [SerializeField] private GameObject[] _sortLayers;
        [SerializeField] private List<UICharacterCardButton> _cardButtons;
        [SerializeField] private GameObject _sortKeysUi;

        private UICharacterCardButton _selectedCardButtonHolder;

        private int _currentIndex = 0;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _party.Members.Length;
                _currentIndex = (value + count) % count;
            }
        }

        private int _indexHolder;
        private IParty _party;

        private void OnValidate()
        {
            if (_partySlots.Length != PartyConstants.MAX_PARTY_SIZE)
            {
                Array.Resize(ref _partySlots, PartyConstants.MAX_PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<UIPartySlot>();
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

        private void OnEnableSortMode()
        {
            _selectedCardButtonHolder = _partySlots[CurrentIndex].transform.GetChild(0).GetComponent<UICharacterCardButton>();
            _selectedCardButtonHolder.Select();
        }

        public void ConfirmSelect(UICharacterCardButton card)
        {
            SelectedEvent?.Invoke();

            _topLine.SetActive(false);
            _sortKeysUi.SetActive(true);

            CurrentIndex = _partySlots.ToList().IndexOf(card.transform.parent.GetComponent<UIPartySlot>());
            _selectedCardButtonHolder = card;

            PutToSortLayer(_selectedCardButtonHolder.transform);
        }

        public void SwapRight()
        {
            var currentTarget = _sortLayers[CurrentIndex].transform.GetChild(0);
            var otherTarget = _partySlots[CurrentIndex + 1].transform.GetChild(0);

            PutToNormalLayer(otherTarget, CurrentIndex);
            CurrentIndex++;
            PutToSortLayer(currentTarget);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void SwapLeft()
        {
            var currentTarget = _sortLayers[CurrentIndex].transform.GetChild(0);
            var otherTarget = _partySlots[CurrentIndex - 1].transform.GetChild(0);

            PutToNormalLayer(otherTarget, CurrentIndex);
            CurrentIndex--;
            PutToSortLayer(currentTarget);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        private void PutToSortLayer(Transform targetTransform)
        {
            targetTransform.SetParent(_sortLayers[CurrentIndex].transform);
            targetTransform.localPosition = new Vector3(0, 0, 0);
        }

        private void PutToNormalLayer(Transform targetTransform, int targetIndex)
        {
            targetTransform.SetParent(_partySlots[targetIndex].transform);
            targetTransform.localPosition = new Vector3(0, 0, 0);
        }

        public void ConfirmSortOrder()
        {
            _topLine.SetActive(true);

            PutToNormalLayer(_selectedCardButtonHolder.transform, CurrentIndex);
            Invoke(nameof(OnConfirmSortOrder), .2f);
            _selectedCardButtonHolder.BackToNormalState();
        }

        private void OnConfirmSortOrder()
        {
            ConfirmedEvent?.Invoke();
        }

        public void CancelSort()
        {
            ApplyDataBeforeSort();
        }

        private void ApplyDataBeforeSort()
        {
            var currentTarget = _characterSlots.GetChild(CurrentIndex);
            currentTarget.SetSiblingIndex(_indexHolder);

            CurrentIndex = _indexHolder;
        }

        private void SetButtonsActive(bool isEnable)
        {
            foreach (var button in _cardButtons)
            {
                button.enabled = isEnable;
            }
        }

        public void Init()
        {
            OnEnableSortMode();
            SetButtonsActive(true);
        }

        public void DeInit()
        {
            SetButtonsActive(false);
            _sortKeysUi.SetActive(false);
        }

        public void SetDefaultSelection()
        {
            CurrentIndex = 0;
        }
    }
}