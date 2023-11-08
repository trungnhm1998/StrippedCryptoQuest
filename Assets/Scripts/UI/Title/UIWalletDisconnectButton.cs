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
    public class UIWalletDisconnectButton : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _connectWalletToken;
        private TinyMessageSubscriptionToken _disconnectWalletToken;

        private void Awake()
        {
            _connectWalletToken = ActionDispatcher.Bind<ConnectWalletCompleted>(OnConnectWalletCompleted);
            _disconnectWalletToken = ActionDispatcher.Bind<DisconnectWalletWalletCompleted>(OnDisconnectWalletCompleted);
        }

        private void OnDestroy()
        {
            ActionDispatcher.Unbind(_connectWalletToken);
            ActionDispatcher.Unbind(_disconnectWalletToken);
        }

        private void Start()
        {
            var credentials =  ServiceProvider.GetService<Credentials>();
            if(credentials != null && string.IsNullOrEmpty(credentials.Profile.user.walletAddress))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnConnectWalletCompleted(ConnectWalletCompleted ctx)
        {
            gameObject.SetActive(ctx.IsSuccess);
        }

        private void OnDisconnectWalletCompleted(DisconnectWalletWalletCompleted ctx)
        {
            gameObject.SetActive(!ctx.IsSuccess);
        }

        public void RequestWalletDisconnect() => ActionDispatcher.Dispatch(new DisconnectWallet());
    }
}
