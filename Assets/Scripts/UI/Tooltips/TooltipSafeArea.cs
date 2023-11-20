using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class TooltipSafeArea : MonoBehaviour
    {
        private static readonly Vector3[] Corners = new Vector3[4];

        public static Vector2 BottomLeftCorner => new(Corners[0].x, Corners[0].y);
        public static Vector2 TopLeftCorner => new(Corners[1].x, Corners[1].y);
        public static Vector2 TopRightCorner => new(Corners[2].x, Corners[2].y);
        public static Vector2 BottomRightCorner => new(Corners[3].x, Corners[3].y);


        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _rectTransform.GetWorldCorners(Corners);
        }
    }
}