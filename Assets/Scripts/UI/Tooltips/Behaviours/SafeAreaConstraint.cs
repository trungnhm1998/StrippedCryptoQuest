using UnityEngine;

namespace CryptoQuest.UI.Tooltips.Behaviours
{
    public class SafeAreaConstraint : TooltipBehaviourBase
    {
        private void OnEnable() => Setup();

        public virtual void Setup()
        {
            var pivotX = RectTransform.pivot.x;
            RectTransform.pivot = new Vector2(pivotX, CalculatePivotYBasedOnSafeArea());
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

        private void Update() => Setup();
    }
}