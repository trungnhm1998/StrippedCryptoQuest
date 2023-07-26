using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenu : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;
        [SerializeField] private VoidEventChannelSO _enableChangeEquipmentModeEvent;

        private void OnEnable()
        {
            _inputMediator.EnableStatusMenuInput();
            _inputMediator.ChangeEquipmentEvent += OnEnableChangeEquipmentMode;
        }

        private void OnDisable()
        {
            _inputMediator.ChangeEquipmentEvent -= OnEnableChangeEquipmentMode;
        }

        private void OnEnableChangeEquipmentMode()
        {
            _enableChangeEquipmentModeEvent.RaiseEvent();
        }

        private void LoadCurrentCharacter() { }
    }
}
