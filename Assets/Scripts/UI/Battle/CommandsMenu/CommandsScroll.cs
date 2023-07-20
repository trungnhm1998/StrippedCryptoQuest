using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class CommandsScroll : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private RectTransform _firstButton;

        [SerializeField] private RectTransform _lastButton;

        [SerializeField] private ScrollRect _scrollRect;

        [SerializeField] private GameObject _arrowUp;
        [SerializeField] private GameObject _arrowDown;

        private void OnEnable()
        {
            _inputMediator.MenuNavigateEvent += CheckButtonPosition;
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
                ScrollToTarget(_firstButton);
                _arrowDown.SetActive(true);
                _arrowUp.SetActive(false);
            }
            else if (currentButton == _lastButton.gameObject)
            {
                ScrollToTarget(_lastButton);
                _arrowUp.SetActive(true);
                _arrowDown.SetActive(false);
            }
        }

        private float ScrollToTarget(RectTransform targetRect, float align = 0)
        {
            float contentHeight = _scrollRect.content.rect.height;
            float viewportHeight = _scrollRect.viewport.rect.height;

            if (contentHeight < viewportHeight) return 0;

            float targetPosition = contentHeight + GetPositionY(targetRect) + targetRect.rect.height * align;
            float gap = viewportHeight * align;
            float normalizedPosition = (targetPosition - gap) / (contentHeight - viewportHeight);

            normalizedPosition = Mathf.Clamp01(normalizedPosition);
            _scrollRect.verticalNormalizedPosition = normalizedPosition;
            return normalizedPosition;
        }

        private float GetPositionY(RectTransform target)
        {
            return target.localPosition.y + target.rect.y;
        }
    }
}