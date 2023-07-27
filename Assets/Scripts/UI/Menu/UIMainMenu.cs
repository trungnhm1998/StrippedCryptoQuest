using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _enableMainMenuInputs;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        [SerializeField] private Button _defaultSelectMenu;

        private void OnEnable()
        {
            _inputMediator.OpenMainMenuEvent += Open;
            _inputMediator.CancelEvent += Close;
            _enableMainMenuInputs.EventRaised += EnableInputs;
        }

        private void OnDisable()
        {
            _inputMediator.OpenMainMenuEvent -= Open;
            _inputMediator.CancelEvent -= Close;
            _enableMainMenuInputs.EventRaised -= EnableInputs;
        }

        private void Open()
        {
            EnableInputs();
            _contents.SetActive(true);
            _defaultSelectMenu.Select();
        }

        private void Close()
        {
            _inputMediator.EnableMapGameplayInput();
            _contents.SetActive(false);
        }

        private void EnableInputs()
        {
            _inputMediator.EnableMenuInput();
        }
    }
}
