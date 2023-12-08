using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Common
{
    [AddComponentMenu("CryptoQuest/UI/Common/SelectFirstChildInList")]
    public class SelectFirstChildInList : MonoBehaviour
    {
        [SerializeField] private bool _selectOnEnable;
        [SerializeField] private float _delay;

        private void OnEnable()
        {
            if (_selectOnEnable) Select();
        }

        public void Select()
        {
            if (!TryGetFirstActiveChild(out var firstChild)) return;
            StartCoroutine(CoSelect(firstChild.gameObject));
        }

        private bool TryGetFirstActiveChild(out Transform firstChild)
        {
            firstChild = null;
            if (transform.childCount == 0) return false;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                firstChild = child;
                return true;
            }

            return false;
        }

        private IEnumerator CoSelect(GameObject firstChild)
        {
            yield return new WaitForSeconds(_delay);
            EventSystem.current.SetSelectedGameObject(firstChild);
        }
    }
}