using UnityEngine;

namespace CryptoQuest.TrainingBattle.State
{
    public class OverviewStateBehaviour : BaseStateBehaviour
    {
        private TrainingBattleStateController _stateController;
        private static readonly int AcceptBattle = Animator.StringToHash("AcceptBattleState");
        private static readonly int CancelBattle = Animator.StringToHash("CancelBattleState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _stateController.DialogController.YesPressedEvent += ChangeState;
            _stateController.DialogController.NoPressedEvent += ExitState;
            _stateController.Input.CancelEvent += ExitState;
            _stateController.IsExitState = false;
        }

        protected override void OnExit()
        {
            _stateController.DialogController.YesPressedEvent -= ChangeState;
            _stateController.DialogController.NoPressedEvent -= ExitState;
            _stateController.Input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            StateMachine.Play(AcceptBattle);
            _stateController.DialogController.ChoiceDialog.Hide();
        }

        private void ExitState()
        {
            StateMachine.Play(CancelBattle);
            _stateController.IsExitState = true;
            _stateController.DialogController.ChoiceDialog.Hide();
        }
    }
}