using System;
using CryptoQuest.Merchant;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.UI
{
    public class UIOverview : MonoBehaviour
    {
        public event Action Closed;

        [SerializeField] private MerchantInput _input;
        [SerializeField] private LocalizedString _strDidYouKnow;
        [SerializeField] private LocalizedString _strWelcome;
        [SerializeField] private GameObject _panel;
        private UIGenericDialog _dialog;

        private void OnEnable()
        {
            _panel.SetActive(false);
            GenericDialogController.Instance.InstantiateAsync(ShowDidYouKnowMessage);
        }

        private void OnDisable()
        {
            _input.CancelEvent -= OnClose;
            if (_dialog) _dialog.Hide();
            GenericDialogController.Instance.Release(_dialog);
            _dialog = null;
        }

        private void ShowDidYouKnowMessage(UIGenericDialog dialog)
        {
            _dialog = dialog;
            _dialog
                .WithMessage(_strDidYouKnow)
                .WithHideCallback(ShowWelcomeMessageAndEnableUI)
                .WithNextMark()
                .RequireInput()
                .Show();
        }

        private void ShowWelcomeMessageAndEnableUI()
        {
            _panel.SetActive(true);
            _dialog
                .WithMessage(_strWelcome)
                .Show();
            _input.CancelEvent += OnClose;
        }

        private void OnClose()
        {
            _input.CancelEvent -= OnClose;
            Closed?.Invoke();
        }
    }
}