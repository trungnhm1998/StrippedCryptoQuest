using CryptoQuest.Events.UI.Dialogs;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public abstract class UITransferSection : MonoBehaviour
    {
        [SerializeField] protected YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private GameObject _contents;
        [SerializeField] private LocalizedString _message;

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
            _yesNoDialogEventSO.SetMessage(_message);
        }

        private void YesButtonPressed()
        {
            // Call API to send items
            Debug.Log($"UITransferSection::Confirm send DBox items");
            _yesNoDialogEventSO.Hide();
        }

        private void NoButtonPressed()
        {
            _yesNoDialogEventSO.Hide();
        }
    }
}
