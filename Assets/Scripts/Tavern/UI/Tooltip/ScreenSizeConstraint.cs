using System;
using UnityEngine;

namespace CryptoQuest.Tavern.UI.Tooltip
{
    public class ScreenSizeConstraint : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var pivotX = _rectTransform.position.x / Screen.width;
            var pivotY = _rectTransform.pivot.y;
            _rectTransform.pivot = new Vector2(pivotX, pivotY);
        }
    }
}