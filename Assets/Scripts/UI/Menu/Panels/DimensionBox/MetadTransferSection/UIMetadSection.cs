using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIMetadSection : UITransferSection
    {
        public static event UnityAction<float, bool> InputValueEvent;
        [SerializeField] private Button _defaultSelection;
        [SerializeField] private List<UIWalletButtons> _walletButtons;
        [SerializeField] private TMP_InputField _inputField;
        private EWalletType _walletType;

        private void OnEnable()
        {
            SubscribeButtonEvents();
            SubscribeInputFieldEvent();
        }

        private void OnDisable()
        {
            UnsubscribeButtonEvents();
            UnsubscribeInputFieldEvent();
        }

        private void SubscribeButtonEvents()
        {
            foreach (var button in _walletButtons)
            {
                button.SelectedEvent += OpenTab;
            }
        }

        private void UnsubscribeButtonEvents()
        {
            foreach (var button in _walletButtons)
            {
                button.SelectedEvent -= OpenTab;
            }
        }

        private void SubscribeInputFieldEvent()
        {
            _inputField.onSubmit.AddListener(SubmitSendMetad);
        }

        private void UnsubscribeInputFieldEvent()
        {
            _inputField.onSubmit.RemoveListener(SubmitSendMetad);
        }

        private void OpenTab(UIWalletButtons button)
        {
            _inputField.Select();
            button.SetHighlight(true);
            _walletType = button.WalletType;
        }

        public void Init()
        {
            EnableAllButtons();
            UnhighlightAllButtons();
            _inputField.text = "0";
            _defaultSelection.Select();
        }

        public void UnhighlightAllButtons()
        {
            foreach (var button in _walletButtons)
            {
                button.SetHighlight(false);
            }
        }

        private void SubmitSendMetad(string input)
        {
            float quantityMetadInput = float.Parse(input);
            InputValueEvent?.Invoke(quantityMetadInput, _walletType == EWalletType.IngameWallet);
        }

        private void EnableAllButtons()
        {
            foreach (var wallet in _walletButtons)
            {
                wallet.Button.interactable = true;
            }
        }

        public void DisableAllButtons()
        {
            foreach (var wallet in _walletButtons)
            {
                wallet.Button.interactable = false;
            }
        }
    }
}