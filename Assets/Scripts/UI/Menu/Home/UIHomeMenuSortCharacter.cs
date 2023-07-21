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
        [SerializeField] private VoidEventChannelSO _partyLoaded;
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private List<UICharacterCard> _charCards;

        private void Awake()
        {
            _inputMediator.EnableMenuInput();
        }

        private void OnEnable()
        {
            _inputMediator.HomeMenuSortEvent += EnableSortFunc;
            _partyLoaded.EventRaised += OnPartyLoaded;
        }

        private void OnDisable()
        {
            _inputMediator.HomeMenuSortEvent -= EnableSortFunc;
            _partyLoaded.EventRaised -= OnPartyLoaded;
        }

        private void OnPartyLoaded()
        {
            GetCurrentCardParty();
        }

        private void GetCurrentCardParty()
        {
            for (int i = 0; i < _characterSlots.childCount; i++)
            {
                var cardUI = _characterSlots.GetChild(i).GetComponent<UICharacterCard>();
                Debug.Log($"cardUI=[{cardUI}]");
                _charCards.Add(cardUI);
            }
        }

        private void EnableSortFunc()
        {
            Debug.Log("Sort");
            _charCards[0].OnBeingSelected();
        }
    }
}
