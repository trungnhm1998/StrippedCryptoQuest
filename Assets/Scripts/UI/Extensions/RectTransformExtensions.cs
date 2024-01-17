using UnityEngine;

namespace CryptoQuest.UI.Extensions
{
    public static class RectTransformExtensions
    {
        public static void DestroyAllChildrenImmediately(this RectTransform rectTransform)
        {
            for (var i = rectTransform.childCount - 1; i >= 0; i--)
            {
                var child = rectTransform.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}