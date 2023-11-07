using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class UIWalletButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private TinyMessageSubscriptionToken _showWalletButtonEvent;

        private void OnEnable() => _showWalletButtonEvent = ActionDispatcher.Bind<ShowWalletButton>(ShowWalletButton);

        private void OnDisable() => ActionDispatcher.Unbind(_showWalletButtonEvent);

        private void ShowWalletButton(ShowWalletButton ctx)
        {
            gameObject.SetActive(ctx.IsShown);
        }

        public void RequestWalletLogin() => ActionDispatcher.Dispatch(new LoginUsingWallet());
    }
}
