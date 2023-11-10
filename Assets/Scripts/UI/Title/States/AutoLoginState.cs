using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using TinyMessenger;

namespace CryptoQuest.UI.Title.States
{
    public class AutoLoginState : IState
    {
        private TitleStateMachine _stateMachine;
        private TinyMessageSubscriptionToken _autoLoginFailed;
        private TinyMessageSubscriptionToken _authSucceed;
        private TinyMessageSubscriptionToken _authFailed;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _authSucceed = ActionDispatcher.Bind<AuthenticateSucceed>(OnAuthenticateSucceed);
            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(OnAuthenticateFailed);
            _autoLoginFailed = ActionDispatcher.Bind<SNSAutoLoginFailed>(OnSNSAutoLoginFailed);
            ActionDispatcher.Dispatch(new SNSAutoLogin());
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            ActionDispatcher.Unbind(_authSucceed);
            ActionDispatcher.Unbind(_authFailed);
            ActionDispatcher.Unbind(_autoLoginFailed);
        }

        private void OnSNSAutoLoginFailed(SNSAutoLoginFailed _) => _stateMachine.ChangeState(new TitleState());

        private void OnAuthenticateFailed(AuthenticateFailed _) => _stateMachine.ChangeState(new TitleState()); 

        private void OnAuthenticateSucceed(AuthenticateSucceed _) => _stateMachine.ChangeState(new StartGameState());
    }
}