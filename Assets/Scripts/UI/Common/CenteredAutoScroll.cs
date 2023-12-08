using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    [RequireComponent(typeof(ScrollRect))]
    public class CenteredAutoScroll : MonoBehaviour
    {
        ScrollRect _scrollRect;
        RectTransform _rectTransform;
        RectTransform _contentRectTransform;
        RectTransform _selectedRectTransform;

        void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _rectTransform = GetComponent<RectTransform>();
            _contentRectTransform = _scrollRect.content;
        }

        void Update()
        {
            UpdateScrollToSelected();
        }

        void UpdateScrollToSelected()
        {
            var selected = EventSystem.current.currentSelectedGameObject;

            if (selected == null) return;
            if (selected.transform.parent != _contentRectTransform.transform) return;

            _selectedRectTransform = selected.GetComponent<RectTransform>();
            _scrollRect.ScrollToCenter(_selectedRectTransform);
        }
    }
}