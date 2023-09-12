using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UIHomeMenuSortCharacter : MonoBehaviour
    {
        public event Action SelectedEvent;
        public event Action ConfirmedEvent;
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private VoidEventChannelSO _sortFailedEvent;

        [Header("Game Components")]
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
            _sortFailedEvent.EventRaised += ResetSortOrder;

            _sortKeysUi.SetActive(true);
            LoadPartyMembers();
        }

        private void OnDisable()
        {
            _serviceProvider.PartyProvided -= InitParty;
            UICharacterCardButton.SelectedEvent -= ConfirmSelect;
            _sortFailedEvent.EventRaised -= ResetSortOrder;
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

        private void EnableSortMode()
        {
            _selectedCardButtonHolder = _partySlots[CurrentIndex].transform.GetChild(0).GetComponent<UICharacterCardButton>();
            _selectedCardButtonHolder.Select();
        }

        private void SetButtonsActive(bool isEnable)
        {
            foreach (var button in _cardButtons)
            {
                button.enabled = isEnable;
            }
        }

        #region State involved methods
        public void Init()
        {
            EnableSortMode();
            SetButtonsActive(true);
        }

        public void DeInit()
        {
            SetButtonsActive(false);
            _sortKeysUi.SetActive(false);
        }

        public void ConfirmSelect(UICharacterCardButton card)
        {
            SelectedEvent?.Invoke();

            _topLine.SetActive(false);
            _sortKeysUi.SetActive(true);

            CurrentIndex = _partySlots.ToList().IndexOf(card.transform.parent.GetComponent<UIPartySlot>());
            _indexHolder = CurrentIndex;
            _selectedCardButtonHolder = card;

            PutToSortLayer(_selectedCardButtonHolder.transform);
        }

        public void SwapRight()
        {
            Transform currentTarget = _sortLayers[CurrentIndex].transform.GetChild(0);
            Transform otherTarget = _partySlots[CurrentIndex + 1].transform.GetChild(0);

            PutToNormalLayer(otherTarget, CurrentIndex);
            CurrentIndex++;
            PutToSortLayer(currentTarget);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void SwapLeft()
        {
            Transform currentTarget = _sortLayers[CurrentIndex].transform.GetChild(0);
            Transform otherTarget = _partySlots[CurrentIndex - 1].transform.GetChild(0);

            PutToNormalLayer(otherTarget, CurrentIndex);
            CurrentIndex--;
            PutToSortLayer(currentTarget);

            _selectedCardButtonHolder = currentTarget.GetComponent<UICharacterCardButton>();
        }

        public void ConfirmSortOrder()
        {
            _topLine.SetActive(true);

            PutToNormalLayer(_selectedCardButtonHolder.transform, CurrentIndex);

            _party.Sort(_indexHolder, CurrentIndex);

            // Must delay a bit (0.2s) to avoid bug caused by exiting SortState immediately
            Invoke(nameof(OnConfirmSortOrder), .2f);
            _selectedCardButtonHolder.BackToNormalState();
        }

        public void CancelSort()
        {
            ResetSortOrder();
            _selectedCardButtonHolder.BackToNormalState();
        }

        public void SetDefaultSelection()
        {
            CurrentIndex = 0;
        }
        #endregion

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

        private void OnConfirmSortOrder()
        {
            ConfirmedEvent?.Invoke();
        }

        private void ResetSortOrder()
        {
            var otherTarget = _partySlots[_indexHolder].transform.GetChild(0);

            PutToNormalLayer(otherTarget, CurrentIndex);
            PutToNormalLayer(_selectedCardButtonHolder.transform, _indexHolder);

            CurrentIndex = _indexHolder;
        }
    }
}