using UnityEngine;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class SafeAreaConstraint : TooltipBehaviourBase
    {
        public override void Setup()
        {
            var pivotX = RectTransform.pivot.x;
            RectTransform.pivot = new Vector2(pivotX, CalculatePivotYBasedOnSafeArea());
        }

        private float CalculatePivotYBasedOnSafeArea()
        {
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            var bottomY = corners[0].y;
            var safeAreaBottomLeftCorner = TooltipSafeArea.BottomLeftCorner;
            if (bottomY < safeAreaBottomLeftCorner.y) return 0;
            return RectTransform.pivot.y;
        }
    }
}