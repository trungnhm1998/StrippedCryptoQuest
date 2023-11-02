using System;
using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.UI.Common;
using TinyMessenger;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UISocialPanel : MonoBehaviour
    {
        private Button[] _buttons;
        private TinyMessageSubscriptionToken _authFailed;

        private void OnEnable()
        {
            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(_ => EnableAllButtons());
            EnableAllButtons();
            GetComponentInChildren<SelectButtonOnEnable>().Select();
        }

        private void OnDisable()
        {
            EnableAllButtons(false);
            ActionDispatcher.Unbind(_authFailed);
        }

        public void RequestFacebookLogin() => PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingFacebook()));

        public void RequestWalletLogin() => PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingWallet()));

        public void RequestTwitterLogin() => PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingTwitter()));

        public void RequestGmailLogin() => PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingGoogle()));

        public void RequestEmailAndPasswordLogin() => PreventDoubleDispatch(() => ActionDispatcher.Dispatch(new LoginUsingEmail()));

        private void PreventDoubleDispatch(Action callback)
        {
            EnableAllButtons(false);
            callback();
        }

        private void EnableAllButtons(bool isEnabled = true)
        {
            _buttons ??= GetComponentsInChildren<Button>();
            foreach (var button in _buttons) button.interactable = isEnabled;
        }
    }
}