using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using TinyMessenger;

namespace CryptoQuest.UI.Title.States
{
    public class TitleState : IState
    {
        private UISocialPanel _socialPanel;
        private TinyMessageSubscriptionToken _loginUsingEmailEvent;
        private TitleStateMachine _stateMachine;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.TryGetComponentInChildren(out _socialPanel);
            _socialPanel.gameObject.SetActive(true);

            _loginUsingEmailEvent = ActionDispatcher.Bind<LoginUsingEmail>(ChangeLoginUsingEmailState);
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            ActionDispatcher.Unbind(_loginUsingEmailEvent);
            _socialPanel.gameObject.SetActive(false);
        }

        private void ChangeLoginUsingEmailState(LoginUsingEmail _)
        {
            _stateMachine.ChangeState(new MailLoginState());
        }
    }
}