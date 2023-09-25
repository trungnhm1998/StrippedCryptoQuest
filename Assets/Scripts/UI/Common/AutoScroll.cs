using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class AutoScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        [Tooltip("Template item of the scroll view")]
        [SerializeField] private RectTransform _singleItemRect;

        [Tooltip("Enable this flag to run the auto-scroll logic in the Update()")]
        [SerializeField] private bool _useUpdate = true;

        private RectTransform _viewport;
        private float _verticalOffset;
        private float _lowerBound;
        private float _upperBound;
        private GameObject _current;

        private void Awake()
        {
            _verticalOffset = _singleItemRect.rect.height;
            _viewport = _scrollRect.viewport;

            CalculateBoundariesByHeight();
        }

        private void Update()
        {
            if (!_useUpdate) return;
            _current = EventSystem.current.currentSelectedGameObject;
            if (IsBeingUsed()) return;
            Scroll();
        }

        private float NormalizePositionFromDifferentPivots(Rect rect)
        {
            if (_viewport.pivot.y == 0)
                return _viewport.position.y + (rect.height / 2);
            else if (_viewport.pivot.y == 0.5)
                return _viewport.position.y;
            else
                return _viewport.position.y - (rect.height / 2);
        }

        private void CalculateBoundariesByHeight()
        {
            var rect = _viewport.rect;
            float yPosViewport = NormalizePositionFromDifferentPivots(rect);

            _lowerBound = yPosViewport - rect.height / 2 + _verticalOffset;
            _upperBound = yPosViewport + rect.height / 2 + _verticalOffset;
        }

        /// <summary>
        /// Check if the currently selected items belong to the scroll view that you want to use.
        /// </summary>
        private bool IsBeingUsed()
        {
            return _current.transform.parent != _scrollRect.content;
        }

        public void Scroll()
        {
            var selectedRowPositionY = _current.transform.position.y;
            ScrollUpIfOutOfLowerBound(selectedRowPositionY);
            ScrollDownIfOutOfUpperBound(selectedRowPositionY);
        }

        private void ScrollUpIfOutOfLowerBound(float selectedRowPositionY)
        {
            if (selectedRowPositionY <= _lowerBound)
                _scrollRect.content.anchoredPosition += Vector2.up * _verticalOffset;
        }

        private void ScrollDownIfOutOfUpperBound(float selectedRowPositionY)
        {
            if (selectedRowPositionY >= _upperBound)
                _scrollRect.content.anchoredPosition += Vector2.down * _verticalOffset;
        }
    }
}