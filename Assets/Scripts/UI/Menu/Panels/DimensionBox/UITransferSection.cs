using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public abstract class UITransferSection : MonoBehaviour
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

        public virtual void ResetTransfer() { }
    }
}
