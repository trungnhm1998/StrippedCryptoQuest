using System;
using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.MockData;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialog
{
    public class SpeechDialog : MonoBehaviour
    {
        [Header("Events received")]
        [SerializeField] private VoidEventChannelSO _turnOnDialogEvent;
        [SerializeField] private VoidEventChannelSO _turnOffDialogEvent;

        [Space]
        [SerializeField] private NPCSpeechSO _receivedMessage;
        [SerializeField] private GameObject _innerUI;
        [SerializeField] private Text _message;

        private void OnEnable()
        {
            _turnOnDialogEvent.EventRaised += TurnOnDialog;
            _turnOffDialogEvent.EventRaised += TurnOffDialog;
        }

        private void OnDisable()
        {
            _turnOnDialogEvent.EventRaised -= TurnOnDialog;
            _turnOffDialogEvent.EventRaised -= TurnOffDialog;
        }

        private void TurnOnDialog()
        {
            SetupMessage();
            _innerUI.SetActive(true);
        }

        private void TurnOffDialog()
        {
            _innerUI.SetActive(false);
        }

        private void SetupMessage()
        {
            _message.text = $"{_receivedMessage.Message}";
        }
    }
}
