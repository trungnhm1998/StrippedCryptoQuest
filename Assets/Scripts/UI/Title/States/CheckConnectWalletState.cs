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
        private TinyMessageSubscriptionToken _fetchProfileSuccessToken;
        private TinyMessageSubscriptionToken _fetchProfileFailToken;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            ActionDispatcher.Dispatch(new CheckConnectWallet());
            if (string.IsNullOrEmpty(_stateMachine.Credentials.Wallet)) 
            {
                CheckConnectedWalletFail();
                return;
            }

            _fetchProfileSuccessToken = ActionDispatcher.Bind<ConnectedWallet>(_ 
                => ConnectedWallet());
            _fetchProfileFailToken = ActionDispatcher.Bind<ConnectedWalletFailed>(_ 
                => _stateMachine.ChangeState(new TitleState()));

            // To check if token is expired or not
            ActionDispatcher.Dispatch(new CheckConnectWallet());
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            if (_fetchProfileSuccessToken != null)
                ActionDispatcher.Unbind(_fetchProfileSuccessToken);
            if (_fetchProfileFailToken != null)
                ActionDispatcher.Unbind(_fetchProfileFailToken);
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