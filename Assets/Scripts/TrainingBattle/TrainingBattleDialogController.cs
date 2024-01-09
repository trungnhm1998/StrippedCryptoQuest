using System;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.TrainingBattle
{
    public class TrainingBattleDialogController : MonoBehaviour
    {
        public event Action YesPressedEvent;
        public event Action NoPressedEvent;
        [SerializeField] private LocalizedString _defaultMessage;
        public UIDialogueForGenericMerchant Dialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        public void ShowDialog()
        {
            GenericMerchantDialogueController.Instance.InstantiateAsync(dialog => Dialogue = dialog);
            ChoiceDialogController.Instance.InstantiateAsync(ChoiceDialogInstantiated);
        }

        public void HideDialog()
        {
            GenericMerchantDialogueController.Instance.Release(Dialogue);
            ChoiceDialogController.Instance.Release(ChoiceDialog);
        }

        private void ChoiceDialogInstantiated(UIChoiceDialog dialog)
        {
            ChoiceDialog = dialog;
            ShowChoiceDialog(_defaultMessage);
        }

        public void ShowChoiceDialog(LocalizedString message)
        {
            ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(message)
                .Show();
        }

        private void YesButtonPressed()
        {
            ChoiceDialog.Hide();
            YesPressedEvent?.Invoke();
        }

        private void NoButtonPressed()
        {
            ChoiceDialog.Hide();
            NoPressedEvent?.Invoke();
        }
    }
}