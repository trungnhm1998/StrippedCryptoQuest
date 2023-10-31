using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public abstract class UIAbstractTavern : MonoBehaviour
    {
        [SerializeField] private GameObject _contents;

        public virtual void EnterTransferSection()
        {
            _contents.SetActive(true);
        }

        public virtual void ExitTransferSection()
        {
            _contents.SetActive(false);
        }

        public abstract void ResetTransfer();

        public abstract void SendItems();

        protected virtual void YesButtonPressed() { }

        protected virtual void NoButtonPressed() { }
    }
}