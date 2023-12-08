using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Common
{
    /// <summary>
    /// To select last managed button in a list, add this component to the list's parent.
    /// </summary>
    [AddComponentMenu("CryptoQuest/UI/Common/CacheButtonSelector")]
    public class CacheButtonSelector : MonoBehaviour
    {
        [SerializeField] private bool _resetOnDisable;

        private void OnDisable()
        {
            if (_resetOnDisable) _lastSelected = null;
        }

        private GameObject _lastSelected;
        public GameObject LastSelected
        {
            get
            {
                if (_lastSelected == null || _lastSelected.activeSelf == false)
                    _lastSelected = null;
                return _lastSelected;
            }
        }

        private void Update()
        {
            var currentSelectedButton = EventSystem.current.currentSelectedGameObject;
            // cache this if current are child of current transform
            if (currentSelectedButton == null) return;
            if (currentSelectedButton.transform.IsChildOf(transform) == false) return;
            _lastSelected = currentSelectedButton;
        }
    }
}