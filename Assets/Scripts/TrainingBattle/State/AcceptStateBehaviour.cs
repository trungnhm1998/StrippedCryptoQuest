using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.TrainingBattle.State
{
    public class AcceptStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private TrainingBattleStateController _stateController;
        private static readonly int EnterTraining = Animator.StringToHash("TrainingBattleState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Show();
            _stateController.Input.SubmitEvent += ChangeState;
            _stateController.Input.CancelEvent += ChangeState;
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
            _stateController.Input.CancelEvent -= ChangeState;
        }

        private void ChangeState()
        {
            StateMachine.Play(EnterTraining);
            _stateController.DialogController.Dialogue.Hide();
        }
    }
}