using UnityEngine;

namespace CryptoQuest.UI.Dialogs
{
    public abstract class AbstractDialog : MonoBehaviour
    {
        [SerializeField] private bool _autoRelease = true;
        [SerializeField] private float _releaseTimeout = 5f;
        [SerializeField] private GameObject _content;
        protected GameObject Content => _content;

        public virtual void Show() => _content.SetActive(true);

        public virtual void Hide()
        {
            _content.SetActive(false);

            if (_autoRelease)
            {
                Invoke(nameof(Release), _releaseTimeout);
            }
        }

        public void Release()
        {
            Destroy(gameObject);
        }
    }
}