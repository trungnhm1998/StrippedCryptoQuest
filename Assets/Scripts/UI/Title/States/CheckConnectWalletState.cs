using CryptoQuest.Sagas.Profile;
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
        private TinyMessageSubscriptionToken _checkSuccessToken;
        private TinyMessageSubscriptionToken _checkFailToken;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            if (string.IsNullOrEmpty(_stateMachine.Credentials.Wallet)) 
            {
                CheckConnectedWalletFail();
                return;
            }

            _checkSuccessToken = ActionDispatcher.Bind<ConnectedWallet>(_ 
                => ConnectedWallet());
            _checkFailToken = ActionDispatcher.Bind<ConnectedWalletFailed>(_ 
                => _stateMachine.ChangeState(new TitleState()));

            // To check if token is expired or not
            ActionDispatcher.Dispatch(new CheckConnectWallet());
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            if (_checkSuccessToken != null)
                ActionDispatcher.Unbind(_checkSuccessToken);
            if (_checkFailToken != null)
                ActionDispatcher.Unbind(_checkFailToken);
        }

        private void ConnectedWallet()
        {
            if (!IsPlayerNameExist())
            {
                _stateMachine.ChangeState(new NameInputState());
                return;
            }
            
            OnExit(_stateMachine);
            ActionDispatcher.Dispatch(new StartGameAction());
        }

        private bool IsPlayerNameExist()
        {
            var saveSystem = ServiceProvider.GetService<SaveSystemSO>();
            return !string.IsNullOrEmpty(saveSystem.PlayerName);
        }

        private void CheckConnectedWalletFail()
        {
            ActionDispatcher.Dispatch(new NotConnectWalletErrorPopup());
            _stateMachine.ChangeState(new StartGameState());
        }
    }
}