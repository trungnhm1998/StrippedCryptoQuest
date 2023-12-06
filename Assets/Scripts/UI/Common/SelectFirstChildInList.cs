using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Common
{
    public class SelectFirstChildInList : MonoBehaviour
    {
        [SerializeField] private float _delay;

        public void Select()
        {
            if (!TryGetChild(out var firstChild)) return;
            StartCoroutine(CoSelect(firstChild));
        }

        private bool TryGetChild(out Transform firstChild) =>
            firstChild = transform.childCount > 0 ? transform.GetChild(0) : firstChild = null;

        private IEnumerator CoSelect(Transform firstChild)
        {
            yield return new WaitForSeconds(_delay);
            EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
        }
    }
}