using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenu : UIMenuPanel
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _enableChangeEquipmentModeEvent;


        private void OnEnable()
        {
            _inputMediator.EnableStatusMenuInput();
            _inputMediator.StatusMenuCancelEvent += InputMediatorOnStatusMenuCancelEvent;

            _inputMediator.EnableChangeEquipmentModeEvent += OnEnableEnableChangeEquipmentModeMode;
        }

        private void OnDisable()
        {
            _inputMediator.StatusMenuCancelEvent -= InputMediatorOnStatusMenuCancelEvent;
            _inputMediator.EnableChangeEquipmentModeEvent -= OnEnableEnableChangeEquipmentModeMode;
        }


        private void InputMediatorOnStatusMenuCancelEvent()
        {
            _inputMediator.EnableMenuInput();
        }

        private void OnEnableEnableChangeEquipmentModeMode()
        {
            _enableChangeEquipmentModeEvent.RaiseEvent();
        }
    }
}