using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menu
{
    // Add the script to your Dropdown Menu Template Object via (Your Dropdown Button > Template)

    [RequireComponent(typeof(ScrollRect))]
    public class AutoScrollRect : MonoBehaviour
    {
        // Sets the speed to move the scrollbar
        [SerializeField] private float _viewportValueAbove = 0f;
        [SerializeField] private float _viewportValueBelow = 0f;

        // Set as Template Object via (Your Dropdown Button > Template)
        [SerializeField] private ScrollRect _templateScrollRect;

        // Set as Template Viewport Object via (Your Dropdown Button > Template > Viewport)
        [SerializeField] private RectTransform _templateViewportTransform;

        // Set as Template Content Object via (Your Dropdown Button > Template > Viewport > Content)
        public RectTransform ContentRectTransform;

        private RectTransform _selectedRectTransform;

        private void UpdateScrollToSelected(ScrollRect scrollRect, RectTransform contentRectTransform, RectTransform viewportRectTransform)
        {
            // Get the current selected option from the eventsystem.
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected == null || selected.transform.parent != contentRectTransform.transform)
            {
                return;
            }
            _selectedRectTransform = selected.GetComponent<RectTransform>();

            // Math stuff
            var selectedDifference = viewportRectTransform.localPosition - _selectedRectTransform.localPosition;
            var contentHeightDifference = contentRectTransform.rect.height - viewportRectTransform.rect.height;

            var selectedPosition = contentRectTransform.rect.height - selectedDifference.y;
            var currentScrollRectPosition = scrollRect.normalizedPosition.y * contentHeightDifference;
            var above = currentScrollRectPosition - _selectedRectTransform.rect.height / 2 + (viewportRectTransform.rect.height / _viewportValueAbove);
            var below = currentScrollRectPosition + _selectedRectTransform.rect.height / 2 - (viewportRectTransform.rect.height / _viewportValueBelow);

            // Check if selected option is out of bounds.
            if (selectedPosition > above)
            {
                var step = selectedPosition - above;
                var newY = currentScrollRectPosition + step;
                var newNormalizedY = newY / contentHeightDifference;
                scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
            }
            else if (selectedPosition < below)
            {
                var step = selectedPosition - below;
                var newY = currentScrollRectPosition + step;
                var newNormalizedY = newY / contentHeightDifference;
                scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
            }
        }
        public void UpdateScrollRectTransform()
        {
            UpdateScrollToSelected(_templateScrollRect, ContentRectTransform, _templateViewportTransform);
        }
    }
}