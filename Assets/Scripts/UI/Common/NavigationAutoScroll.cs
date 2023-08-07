using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class NavigationAutoScroll : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _firstButton;

        public RectTransform FirstButton
        {
            set => _firstButton = value;
        }

        [SerializeField] private RectTransform _lastButton;

        public RectTransform LastButton
        {
            set => _lastButton = value;
        }

        [SerializeField] private GameObject _arrowUpHint;
        [SerializeField] private GameObject _arrowDownHint;

        [Header("Panel")]
        [SerializeField] private ScrollRect _scrollRect;

        protected virtual void Update()
        {
            CheckButtonPosition();
        }

        private void CheckButtonPosition()
        {
            if (_firstButton == null || _lastButton == null) return;

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