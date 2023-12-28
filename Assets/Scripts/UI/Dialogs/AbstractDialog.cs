using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Dialogs
{
    public abstract class AbstractDialog : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        public GameObject Content => _content;

        public virtual void Show() => _content.SetActive(true);
        public virtual void Hide()
        {
            EventSystem.current.SetSelectedGameObject(null);
            _content.SetActive(false);
        }
    }
}