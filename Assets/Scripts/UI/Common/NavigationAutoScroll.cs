using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class NavigationAutoScroll : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _firstButton;

        [SerializeField] private RectTransform _lastButton;
        [SerializeField] private GameObject _arrowUpHint;
        [SerializeField] private GameObject _arrowDownHint;

        [Header("Panel")]
        [SerializeField] private ScrollRect _scrollRect;


        public void CheckButtonPosition()
        {
            var currentButton = EventSystem.current.currentSelectedGameObject;

            if (currentButton == _firstButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    ScrollRectHelper.CalculateNormalizedPosition(_scrollRect, _firstButton);
                _arrowDownHint.SetActive(true);
                _arrowUpHint.SetActive(false);
            }
            else if (currentButton == _lastButton.gameObject)
            {
                _scrollRect.verticalNormalizedPosition =
                    ScrollRectHelper.CalculateNormalizedPosition(_scrollRect, _lastButton);
                _arrowUpHint.SetActive(true);
                _arrowDownHint.SetActive(false);
            }
        }
    }
}