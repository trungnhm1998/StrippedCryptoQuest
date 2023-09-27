using CryptoQuest.UI.Dialogs.BattleDialog;

namespace CryptoQuest.Battle.States
{
    public class Intro : IState
    {
        private BattleStateMachine _battleStateMachine;

        private readonly UIGenericDialog _dialog;

        public Intro(UIGenericDialog dialog)
        {
            _dialog = dialog;
        }

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            var introUI = _battleStateMachine.IntroUI;
            _dialog
                .WithAutoHide(introUI.Duration)
                .WithHideCallback(() => _battleStateMachine.ChangeState(new SelectCommand()))
                .WithMessage(introUI.IntroMessage)
                .Show();
        }

        public void OnExit(BattleStateMachine battleStateMachine) { }
    }
}