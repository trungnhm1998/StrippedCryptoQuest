using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private MenuSelectionHandler _selectionHandler;

        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

        [SerializeField] private Button _defaultSelectMenu;

        [SerializeField] private UnityEvent MainMenuShow;
        [SerializeField] private UnityEvent MainMenuClose;

        // TODO: DEBUG, REMOVE!
        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
        }

        private void OnEnable()
        {
            _inputMediator.OpenMainMenuEvent += Open;
            _inputMediator.CancelEvent += Close;
        }

        private void OnDisable()
        {
            _inputMediator.OpenMainMenuEvent -= Open;
            _inputMediator.CancelEvent -= Close;
        }

        private void Open()
        {
            EnableInputs();
            _contents.SetActive(true);
            _selectionHandler.UpdateSelection(_defaultSelectMenu.gameObject);
            MainMenuShow.Invoke();
        }

        private void Close()
        {
            MainMenuClose.Invoke();
            _contents.SetActive(false);
            _inputMediator.EnableMapGameplayInput();
        }

        private void EnableInputs()
        {
            _inputMediator.EnableMenuInput();
        }
    }
}