using System;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class NavigationAutoScroll : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private RectTransform _firstButton;

        public RectTransform FirstButton
        {
            get => _firstButton;
            set => _firstButton = value;
        }

        [SerializeField] private RectTransform _lastButton;

        public RectTransform LastButton
        {
            get => _lastButton;
            set => _lastButton = value;
        }

        [SerializeField] private GameObject _arrowUpHint;
        [SerializeField] private GameObject _arrowDownHint;

        [Header("Panel")]
        [SerializeField] private ScrollRect _scrollRect;


        private void OnEnable()
        {
            _inputMediator.MenuNavigateEvent += CheckButtonPosition;
            _inputMediator.CancelEvent += CheckButtonPosition;
        }

        private void OnDisable()
        {
            _inputMediator.MenuNavigateEvent -= CheckButtonPosition;
            _inputMediator.CancelEvent -= CheckButtonPosition;
        }

        private void CheckButtonPosition()
        {
            var currentButton = EventSystem.current.currentSelectedGameObject;

            if (currentButton == _firstButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    _scrollRect.CalculateNormalizedPosition(_firstButton);
                _arrowDownHint.SetActive(true);
                _arrowUpHint.SetActive(false);
            }
            else if (currentButton == _lastButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    _scrollRect.CalculateNormalizedPosition(_lastButton);
                _arrowUpHint.SetActive(true);
                _arrowDownHint.SetActive(false);
            }
        }
    }
}