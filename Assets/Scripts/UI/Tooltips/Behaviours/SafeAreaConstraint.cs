using UnityEngine;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class SafeAreaConstraint : TooltipBehaviourBase
    {
        private void OnEnable() => Setup();

        public virtual void Setup()
        {
            RectTransform.pivot = new Vector2(CalculatePivotXBasedOnSafeArea(), CalculatePivotYBasedOnSafeArea());
        }

        protected float CalculatePivotYBasedOnSafeArea()
        {
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            var bottomY = corners[0].y;
            var safeAreaBottomLeftCorner = TooltipSafeArea.BottomLeftCorner;
            if (bottomY < safeAreaBottomLeftCorner.y) return 0;
            return RectTransform.pivot.y;
        }

        protected float CalculatePivotXBasedOnSafeArea()
        {
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            var leftX = corners[0].x;
            var safeAreaBottomRightCorner = TooltipSafeArea.BottomRightCorner;
            if (leftX < safeAreaBottomRightCorner.x) return 0;
            return RectTransform.pivot.x;
        }

        private void Update() => Setup();
    }
}