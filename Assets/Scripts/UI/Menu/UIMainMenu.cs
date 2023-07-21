using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        [SerializeField] private Button _defaultSelectMenu;

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
            _inputMediator.EnableMenuInput();
            _contents.SetActive(true);
            _defaultSelectMenu.Select();
        }

        private void Close()
        {
            _inputMediator.EnableMapGameplayInput();
            _contents.SetActive(false);
        }
    }
}
