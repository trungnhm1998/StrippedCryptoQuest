using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

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
            _contents.SetActive(true);
            _inputMediator.EnableMenuInput();
        }

        private void Close()
        {
            _contents.SetActive(false);
        }
    }
}
