using System.Collections.Generic;
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
        public event UnityAction EnableOverviewButtonsEvent;

        public UIDialogueForGenericMerchant Dialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        [SerializeField] private MerchantsInputManager _inputManager;
        [SerializeField] private List<LocalizedString> _welcomeMessage = new();

        private int _msgIndex;

        public void TavernOpened()
        {
            _msgIndex = 0;

            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
            ChoiceDialogController.Instance.Instantiate(dialog => ChoiceDialog = dialog, false);

            _inputManager.SubmitEvent += NextDialog;
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            Dialogue = dialog;
            Dialogue
                .SetMessage(_welcomeMessage[0])
                .Show();
        }

        private void NextDialog()
        {
            _msgIndex++;
            if (_msgIndex >= _welcomeMessage.Count)
            {
                EnableOverviewButtonsEvent?.Invoke();
                return;
            }

            Dialogue
                .SetMessage(_welcomeMessage[_msgIndex])
                .Show();
        }

        public void TavernExited()
        {
            GenericMerchantDialogueController.Instance.Release(Dialogue);
            ChoiceDialogController.Instance.Release(ChoiceDialog);
            _inputManager.SubmitEvent -= NextDialog;
        }
    }
}