using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.System;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UIWalletDisconnectButton : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _connectWalletToken;
        private TinyMessageSubscriptionToken _disconnectWalletToken;

        private void Awake()
        {
            _connectWalletToken = ActionDispatcher.Bind<ConnectWalletCompleted>(OnConnectWalletCompleted);
            _disconnectWalletToken = ActionDispatcher.Bind<DisconnectWalletCompleted>(OnDisconnectWalletCompleted);
        }

        private void OnDestroy()
        {
            if (_connectWalletToken != null) ActionDispatcher.Unbind(_connectWalletToken);
            if (_disconnectWalletToken != null) ActionDispatcher.Unbind(_disconnectWalletToken);
        }

        private void Start()
        {
            var credentials = ServiceProvider.GetService<Credentials>();
            if (credentials != null && string.IsNullOrEmpty(credentials.Profile.user.walletAddress))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnConnectWalletCompleted(ConnectWalletCompleted ctx)
        {
            gameObject.SetActive(ctx.IsSuccess);
        }

        private void OnDisconnectWalletCompleted(DisconnectWalletCompleted ctx)
        {
            gameObject.SetActive(!ctx.IsSuccess);
        }

        public void RequestWalletDisconnect() => ActionDispatcher.Dispatch(new DisconnectWallet());
    }
}
