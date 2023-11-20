using UnityEngine;

namespace CryptoQuest.UI.Common
{
    public class RectTransformCornersGUIRenderer : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnGUI()
        {
            var corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);
            var topRightCorner = new Vector2(corners[2].x, corners[2].y);
            var bottomLeftCorner = new Vector2(corners[0].x, corners[0].y);

            var labelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Texture2D.whiteTexture, textColor = Color.black },
                alignment = TextAnchor.MiddleCenter
            };

            var width = 200;
            var height = 30;

            foreach (var corner in corners)
            {
                var v2 = new Vector2(corner.x, corner.y);
                v2.x = Mathf.Min(v2.x, topRightCorner.x - width);
                v2.y = Mathf.Max(v2.y, bottomLeftCorner.y + height);
                GUI.Label(new Rect(v2.x, Screen.height - v2.y, width, height), $"Corner: {v2}", labelStyle);
            }
        }
#endif
    }
}