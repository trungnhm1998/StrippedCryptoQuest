using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class AutoScrollUsingAnchor : AutoScroll
    {
        protected override float GetViewportY() => _viewport.anchoredPosition.y;

        protected override float CalculatedSelectedObjectY(GameObject currentSelected)
        {
            if (!currentSelected.TryGetComponent(out RectTransform rect))
                return base.CalculatedSelectedObjectY(currentSelected);
            return rect.anchoredPosition.y + _scrollRect.content.anchoredPosition.y 
                + _viewport.anchoredPosition.y;
        }
    }
}