using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    [RequireComponent(typeof(AutoScrollViewCalculatorCalculator))]
    public class NavigationAutoScroll : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private RectTransform _firstButton;

        [SerializeField] private RectTransform _lastButton;
        [SerializeField] private GameObject _arrowUpHint;
        [SerializeField] private GameObject _arrowDownHint;

        [Header("Panel")]
        [SerializeField] private ScrollRect _scrollRect;

        private IAutoScrollViewCalculator _autoScrollViewCalculatorCalculation;


        private void OnEnable()
        {
            _inputMediator.MenuNavigateEvent += CheckButtonPosition;
            _autoScrollViewCalculatorCalculation = GetComponent<IAutoScrollViewCalculator>();
        }

        private void OnDisable()
        {
            _inputMediator.MenuNavigateEvent -= CheckButtonPosition;
        }

        private void CheckButtonPosition()
        {
            var currentButton = EventSystem.current.currentSelectedGameObject;

            if (currentButton == _firstButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    _autoScrollViewCalculatorCalculation.CalculateNormalizedScrollPosition(_scrollRect, _firstButton);
                _arrowDownHint.SetActive(true);
                _arrowUpHint.SetActive(false);
            }
            else if (currentButton == _lastButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    _autoScrollViewCalculatorCalculation.CalculateNormalizedScrollPosition(_scrollRect, _lastButton);
                _arrowUpHint.SetActive(true);
                _arrowDownHint.SetActive(false);
            }
        }
    }
}