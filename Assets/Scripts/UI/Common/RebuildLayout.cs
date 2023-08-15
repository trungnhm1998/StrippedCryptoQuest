using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class RebuildLayout : MonoBehaviour
    {
        public RectTransform rectTransform;

        public void Rebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}