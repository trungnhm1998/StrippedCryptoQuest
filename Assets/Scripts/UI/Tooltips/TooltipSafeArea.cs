using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class TooltipSafeArea : MonoBehaviour
    {
        private static Rect _safeArea;

        private static Rect SafeArea
        {
            get
            {
                if (_safeArea is { width: 0, height: 0 })
                {
                    _safeArea = Screen.safeArea;
                }

                return _safeArea;
            }
            set => _safeArea = value;
        }

        public static int Width => (int)SafeArea.width;
        public static int Height => (int)SafeArea.height;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            SafeArea = _rectTransform.rect;
        }

        private void OnDisable()
        {
            SafeArea = Screen.safeArea;
        }

        private void OnGUI()
        {
            // draw mouse position
            var mousePosition = UnityEngine.Input.mousePosition;
            GUI.Label(new Rect(mousePosition.x, Screen.height - mousePosition.y, 100, 100), $"Mouse: {mousePosition}");

            var corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);
            var labelWidth = 200;
            var labelHeight = 30;

            // label style with white background
            var labelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Texture2D.whiteTexture, textColor = Color.black },
                alignment = TextAnchor.MiddleCenter
            };

            foreach (var corner in corners)
            {
                var v2 = new Vector2(corner.x, corner.y);
                if (v2.x + labelWidth > corners[2].x) v2.x = corners[2].x - labelWidth;
                if (v2.y - labelHeight < corners[0].y) v2.y = corners[0].y + labelHeight;
                GUI.Label(new Rect(v2.x, Screen.height - v2.y, labelWidth, labelHeight), $"Corner: {v2}", labelStyle);
            }
        }
    }
}