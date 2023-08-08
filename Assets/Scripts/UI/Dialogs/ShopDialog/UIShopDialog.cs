using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.Shop
{
    public class UIShopDialog : ModalWindow<UIShopDialog>
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Text _dialogText;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _closeShopDialogEventChannel;

        private string _message;


        private void OnEnable()
        {
            _dialogText.text = "";
            _closeShopDialogEventChannel.EventRaised += CloseDialog;
        }

        private void OnDisable()
        {
            _closeShopDialogEventChannel.EventRaised -= CloseDialog;
        }

        private void CloseDialog()
        {
            Close();
        }

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            _inputMediator.EnableDialogueInput();
            _dialogText.text += $"{_message}\n";
        }

        public override UIShopDialog Close()
        {
            gameObject.SetActive(false);
            Visible = false;
            return this;
        }

        public UIShopDialog SetDialogue(string dialogueArgs)
        {
            _message = dialogueArgs;
            return this;
        }
    }
}
