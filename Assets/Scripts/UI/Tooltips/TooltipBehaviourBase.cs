using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public abstract class TooltipBehaviourBase : MonoBehaviour
    {
        private RectTransform _rectTransform;
        protected RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
        public abstract void Setup();
    }
}