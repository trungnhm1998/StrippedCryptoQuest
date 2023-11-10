using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Menu;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Title;
using CryptoQuest.UI.Title.States;
using TinyMessenger;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class UIWalletButton : MonoBehaviour
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
            if (credentials != null && !string.IsNullOrEmpty(credentials.Profile.user.walletAddress))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnConnectWalletCompleted(ConnectWalletCompleted ctx)
        {
            gameObject.SetActive(!ctx.IsSuccess);

            // Need reload StartGame state
            if (ctx.IsSuccess)
            {
                var stateMachine = transform.root.gameObject.GetComponentInChildren<TitleStateMachine>();
                stateMachine.ChangeState(new StartGameState());
            }
        }

        private void OnDisconnectWalletCompleted(DisconnectWalletCompleted ctx)
        {
            gameObject.SetActive(ctx.IsSuccess);
        }

        public void RequestConnectWallet() => ActionDispatcher.Dispatch(new ConnectWallet());
    }
}
