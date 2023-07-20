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
        [SerializeField] private List<GameObject> _menuPointers;

        private void OnEnable()
        {
            _inputMediator.MenuTabPressed += NavigateToNextMenu;
        }

        private void OnDisable()
        {
            _inputMediator.MenuTabPressed -= NavigateToNextMenu;
        }

        private void NavigateToNextMenu()
        {
            Debug.Log("Next");
        }
    }
}
