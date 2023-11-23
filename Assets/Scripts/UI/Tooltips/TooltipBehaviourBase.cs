using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class TooltipBehaviourBase : MonoBehaviour
    {
        private RectTransform _rectTransform;
        protected RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    }
}