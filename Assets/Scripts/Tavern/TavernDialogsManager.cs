using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern
{
    public class TavernDialogsManager : MonoBehaviour
    {
        public event UnityAction TurnOnTavernOptionsEvent;

        public UIDialogueForGenericMerchant Dialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        [SerializeField] private MerchantsInputManager _inputManager;
        [SerializeField] private LocalizedString _didYouKnowMsg;

        public void TavernOpened()
        {
            GenericMerchantDialogueController.Instance.InstantiateAsync(DialogInstantiated);
            ChoiceDialogController.Instance.InstantiateAsync(dialog => ChoiceDialog = dialog);

            _inputManager.SubmitEvent += NextDialog;
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            Dialogue = dialog;
            Dialogue
                .SetMessage(_didYouKnowMsg)
                .Show();
        }

        private void NextDialog() => TurnOnTavernOptionsEvent?.Invoke();

        public void TavernExited()
        {
            GenericMerchantDialogueController.Instance.Release(Dialogue);
            ChoiceDialogController.Instance.Release(ChoiceDialog);
            _inputManager.SubmitEvent -= NextDialog;
        }
    }
}