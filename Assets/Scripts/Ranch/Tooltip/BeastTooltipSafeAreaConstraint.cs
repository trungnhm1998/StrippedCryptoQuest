using CryptoQuest.UI.Tooltips;
using CryptoQuest.UI.Tooltips.Behaviours;
using UnityEngine;

namespace CryptoQuest.Ranch.Tooltip
{
    public class BeastTooltipSafeAreaConstraint : SafeAreaConstraint
    {
        private void OnEnable() => Setup();
        
        public override void Setup()
        {
            RectTransform.pivot = new Vector2(CalculatePivotXBasedOnSafeArea(), CalculatePivotYBasedOnSafeArea());
        }
        
        private float CalculatePivotXBasedOnSafeArea()
        {
            var corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            var leftX = corners[0].x;
            var safeAreaBottomRightCorner = TooltipSafeArea.BottomRightCorner;
            if (leftX < safeAreaBottomRightCorner.x) return 0;
            return RectTransform.pivot.x;
        }
    }
}