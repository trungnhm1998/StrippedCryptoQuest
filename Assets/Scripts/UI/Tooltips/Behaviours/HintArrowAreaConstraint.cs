using UnityEngine;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class HintArrowAreaConstraint : TooltipBehaviourBase
    {
        [SerializeField] private RectTransform _arrowRectTransform;

        private void OnEnable() => Setup();

        private void Setup()
        {
            var isPointingLeft = RectTransform.pivot.x == 0;
            float offsetX = isPointingLeft ? 1 : 0;
            _arrowRectTransform.pivot = new Vector2(offsetX, _arrowRectTransform.pivot.y);
            _arrowRectTransform.anchoredPosition = new Vector2();
        }
    }
}