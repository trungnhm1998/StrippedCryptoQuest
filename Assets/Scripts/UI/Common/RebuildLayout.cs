using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class RebuildLayout : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Rebuild()
        {
            StartCoroutine(CoRebuild());
        }

        private IEnumerator CoRebuild()
        {
            if (_rectTransform == null) yield break;
            yield return null;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
    }
}