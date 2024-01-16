using CryptoQuest.System.SaveSystem;
using CryptoQuest.UI.Popups.Sagas;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.UI.Title.States
{
    public class CheckConnectWalletState : IState
    {
        private TitleStateMachine _stateMachine;
        private TinyMessageSubscriptionToken _walletConnectedToken;
        private TinyMessageSubscriptionToken _walletConnectFailedToken;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            ActionDispatcher.Dispatch(new CheckConnectWallet());
            _walletConnectedToken = ActionDispatcher.Bind<ConnectedWallet>(ConnectedWallet);
            _walletConnectFailedToken = ActionDispatcher.Bind<ConnectedWalletFailed>(CheckConnectedWalletFail);
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            ActionDispatcher.Unbind(_walletConnectedToken);
            ActionDispatcher.Unbind(_walletConnectFailedToken);
        }

        private void ConnectedWallet(ConnectedWallet ctx)
        {
            if (!IsPlayerNameExist())
            {
                _stateMachine.ChangeState(new NameInputState());
                return;
            }

            ActionDispatcher.Dispatch(new StartGameAction());
        }

        private bool IsPlayerNameExist()
        {
            var saveSystem = ServiceProvider.GetService<SaveSystemSO>();
            return !string.IsNullOrEmpty(saveSystem.PlayerName);
        }

        private void CheckConnectedWalletFail(ConnectedWalletFailed ctx)
        {
            ActionDispatcher.Dispatch(new NotConnectWalletErrorPopup());
            _stateMachine.ChangeState(new StartGameState());
        }
    }
}