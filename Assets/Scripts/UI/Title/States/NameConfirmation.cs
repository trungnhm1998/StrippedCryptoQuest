using CryptoQuest.Core;

namespace CryptoQuest.UI.Title.States
{
    public class NameConfirmation : IState
    {
        private UINameConfirmPanel _confirmPanel;
        private TitleStateMachine _stateMachine;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.TryGetComponentInChildren(out _confirmPanel);
            _confirmPanel.gameObject.SetActive(true);
        }


        public void OnExit(TitleStateMachine stateMachine)
        {
            _confirmPanel.gameObject.SetActive(false);
        }

        private void HandleYesClicked()
        {
            _stateMachine.TryGetComponentInChildren(out UINamingPanel namingPanel);
            bool isInputValid = namingPanel.IsInputValid();
            if (!isInputValid)
            {
                _stateMachine.ChangeState(new NameInputState());
                return;
            }

            // ActionDispatcher.Dispatch(new SaveName());
            // _confirmPanel.ConfirmPlayerName();
            // _stateMachine.ChangeState(new PlayGameState(_startGamePanelController));
        }

        private void HandleNoClicked()
        {
            _stateMachine.ChangeState(new NameInputState());
        }
    }
}