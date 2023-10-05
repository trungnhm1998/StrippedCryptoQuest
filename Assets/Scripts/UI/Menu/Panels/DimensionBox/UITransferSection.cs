using CryptoQuest.Events.UI.Dialogs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public abstract class UITransferSection : MonoBehaviour
    {
        [SerializeField] protected YesNoDialogEventChannelSO _yesNoDialogEventSO;
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

        public virtual void SendItems()
        {
            _yesNoDialogEventSO.Show(YesButtonPressed, NoButtonPressed);
        }

        protected virtual void YesButtonPressed()
        {
            _yesNoDialogEventSO.Hide();
        }

        protected virtual void NoButtonPressed()
        {
            _yesNoDialogEventSO.Hide();
        }
    }
}
