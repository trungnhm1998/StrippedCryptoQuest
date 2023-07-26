using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public static class ScrollRectHelper
    {
        public static float CalculateNormalizedPosition(this ScrollRect scrollRect, RectTransform targetRect,
            float align = 0)
        {
            float contentHeight = scrollRect.content.rect.height;
            float viewportHeight = scrollRect.viewport.rect.height;

            if (contentHeight < viewportHeight) return 0;

            float targetPosition = contentHeight + GetPositionY(targetRect) + targetRect.rect.height * align;
            float gap = viewportHeight * align;
            float normalizedPosition = (targetPosition - gap) / (contentHeight - viewportHeight);

            normalizedPosition = Mathf.Clamp01(normalizedPosition);
            return normalizedPosition;
        }

        private static float GetPositionY(this RectTransform rectTransform)
        {
            return rectTransform.localPosition.y + rectTransform.rect.y;
        }
    }
}