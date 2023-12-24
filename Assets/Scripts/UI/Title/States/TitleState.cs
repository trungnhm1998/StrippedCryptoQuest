using CryptoQuest.Actions;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.UI.Title.States
{
    public class TitleState : IState
    {
        private UISocialPanel _socialPanel;
        private TitleStateMachine _stateMachine;
        private TinyMessageSubscriptionToken _authFailed;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.TryGetComponentInChildren(out _socialPanel);
            _socialPanel.gameObject.SetActive(true);

            _authFailed = ActionDispatcher.Bind<AuthenticateFailed>(ToLoginFailed);
        }

        private void ToLoginFailed(AuthenticateFailed _) => _stateMachine.ChangeState(new SocialLoginFailed());

        public void OnExit(TitleStateMachine stateMachine)
        {
            ActionDispatcher.Unbind(_authFailed);
            _socialPanel.gameObject.SetActive(false);
        }
    }
}