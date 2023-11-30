using CryptoQuest.Actions;
using IndiGames.Core.Events;

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
            _confirmPanel.YesPressed += HandleYesClicked;
            _confirmPanel.NoPressed += ChangeToInputNameState;
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            _confirmPanel.YesPressed -= HandleYesClicked;
            _confirmPanel.NoPressed -= ChangeToInputNameState;
            _confirmPanel.gameObject.SetActive(false);
        }

        private void HandleYesClicked()
        {
            _confirmPanel.gameObject.SetActive(false);
            ActionDispatcher.Dispatch(new SaveGameAction());
            ActionDispatcher.Dispatch(new StartGameAction());
        }

        private void ChangeToInputNameState()
        {
            _stateMachine.ChangeState(new NameInputState());
        }
    }
}