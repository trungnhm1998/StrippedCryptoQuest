using CryptoQuest.Input;
using Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Shop.UI.Dialogs
{
    public class UIShopDialog : ModalWindow<UIShopDialog>
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private LocalizeStringEvent _dialogText;

        private void OnEnable()
        {
            _inputMediator.NextDialoguePressed += CloseDialog;
        }

        private void OnDisable()
        {
            _inputMediator.NextDialoguePressed -= CloseDialog;
        }

        private void CloseDialog()
        {
            Close();
            _inputMediator.EnableMapGameplayInput();
        }

        public override UIShopDialog Close()
        {
            gameObject.SetActive(false);
            Visible = false;
            return this;
        }

        public UIShopDialog SetDialogue(LocalizedString dialogueString)
        {
            _dialogText.StringReference = dialogueString;
            return this;
        }
    }
}
