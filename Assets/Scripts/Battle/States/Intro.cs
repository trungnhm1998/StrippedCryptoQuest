using CryptoQuest.Battle.UI.StartBattle;
using CryptoQuest.UI.Dialogs.BattleDialog;

namespace CryptoQuest.Battle.States
{
    public class Intro : IState
    {
        private readonly UIIntroBattle _introBattleUI;
        private UIGenericDialog _dialog;

        public Intro(UIIntroBattle introBattleUI)
        {
            _introBattleUI = introBattleUI;
        }

        private StateMachine _stateMachine;

        public void OnEnter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            GenericDialogController.Instance.CreateDialog(PromptCreated);
        }

        private void PromptCreated(UIGenericDialog dialog)
        {
            _dialog = dialog;
            _dialog
                .WithAutoHide(_introBattleUI.Duration)
                .WithHideCallback(GotoSelectCommandState)
                .WithMessage(_introBattleUI.IntroMessage)
                .Show();
        }

        private void GotoSelectCommandState() => _stateMachine.ChangeState(_stateMachine.Factory.CreateCommand());

        public void OnExit(StateMachine stateMachine) { }
    }
}