using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.UI.Dialogs.Dialogue;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithDialogsPresenter : MonoBehaviour
    {
        public event UnityAction ConfirmYesEvent;
        public event UnityAction ConfirmNoEvent;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private LocalizedString _message;
        private UIDialogueForGenericMerchant _dialogue;
        public UIDialogueForGenericMerchant Dialogue { get => _dialogue; }
        public void BlackSmithOpened()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
        }

        public void BlackSmithClosed()
        {
            Destroy(_dialogue.gameObject); // TODO: code smell here
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            _dialogue = dialog;
            _dialogue
                .SetMessage(_message)
                .Show();
        }

        public void ShowConfirmDialog(LocalizedString confirmMessage)
        {
            _yesNoDialogEventSO.SetMessage(confirmMessage);
            _yesNoDialogEventSO.Show(YesButtonPressed, NoButtonPressed);
        }

        private void YesButtonPressed()
        {
            ConfirmYesEvent?.Invoke();
            HideConfirmDialog();
        }

        private void NoButtonPressed()
        {
            ConfirmNoEvent?.Invoke();
            HideConfirmDialog();
        }

        public void HideConfirmDialog()
        {
            _yesNoDialogEventSO.Hide();
        }
    }
}