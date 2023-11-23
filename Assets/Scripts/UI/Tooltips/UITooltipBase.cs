using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public abstract class UITooltipBase : MonoBehaviour
    {
        public event Action Hide;

        private void OnEnable()
        {
            if (!CanShow())
            {
                OnHide();
                return;
            }

            Init();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            var behaviours = GetComponents<TooltipBehaviourBase>();
            foreach (var behaviour in behaviours) behaviour.Setup();

            // rebuild layout to get the correct size
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        protected abstract bool CanShow();
        private void OnHide() => Hide?.Invoke();
        protected abstract void Init();
    }
}