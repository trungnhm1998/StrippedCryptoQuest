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
            if (GetChild(out var firstChild)) return;
            StartCoroutine(CoSelect(firstChild));
        }

        private bool GetChild(out Transform firstChild)
        {
            firstChild = transform.GetChild(0);
            return firstChild == null;
        }

        private IEnumerator CoSelect(Transform firstChild)
        {
            yield return new WaitForSeconds(_delay);
            EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
        }
    }
}