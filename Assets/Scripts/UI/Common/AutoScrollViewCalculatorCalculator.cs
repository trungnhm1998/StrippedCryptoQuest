using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class AutoScrollViewCalculatorCalculator : MonoBehaviour, IAutoScrollViewCalculator
    {
        public float CalculateNormalizedScrollPosition(ScrollRect scrollRect, RectTransform targetRect, float align = 0)
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

        private float GetPositionY(RectTransform rectTransform)
        {
            return rectTransform.localPosition.y + rectTransform.rect.y;
        }
    }
}