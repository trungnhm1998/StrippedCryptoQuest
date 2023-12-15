using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
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
            StartCoroutine(CoSelect(firstChild));
        }

        private bool TryGetFirstActiveChild(out GameObject firstChild)
        {
            firstChild = null;
            var firstSelectable = GetComponentInChildren<Selectable>();
            if (firstSelectable == null) return false;
            firstChild = firstSelectable.gameObject;
            return firstChild != null;
        }

        private IEnumerator CoSelect(GameObject firstChild)
        {
            yield return new WaitForSeconds(_delay);
            EventSystem.current.SetSelectedGameObject(firstChild);
        }
    }
}