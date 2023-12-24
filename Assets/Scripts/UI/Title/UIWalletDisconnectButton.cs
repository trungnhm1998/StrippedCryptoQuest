using CryptoQuest.Actions;
using CryptoQuest.Networking;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UIWalletDisconnectButton : MonoBehaviour
    {
        [SerializeField] private Credentials _credentials;
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
            if (string.IsNullOrEmpty(_credentials.Wallet))
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
