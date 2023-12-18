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
            StartCoroutine(CoSelect());
        }

        private IEnumerator CoSelect()
        {
            yield return new WaitForSeconds(_delay);
            if (!TryGetFirstActiveChild(out var firstChild)) yield break;
            EventSystem.current.SetSelectedGameObject(firstChild);
        }

        private bool TryGetFirstActiveChild(out GameObject firstChild)
        {
            firstChild = null;
            var firstSelectable = GetComponentInChildren<Selectable>();
            if (firstSelectable == null) return false;
            firstChild = firstSelectable.gameObject;
            return firstChild != null;
        }
    }
}