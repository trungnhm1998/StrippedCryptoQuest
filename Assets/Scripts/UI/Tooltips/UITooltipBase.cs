using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public abstract class UITooltipBase : MonoBehaviour
    {
        private void OnEnable()
        {
            if (!CanShow())
            {
                gameObject.SetActive(false);
                return;
            }

            Init();
            // rebuild layout to get the correct size
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        protected abstract bool CanShow();
        protected abstract void Init();
    }
}