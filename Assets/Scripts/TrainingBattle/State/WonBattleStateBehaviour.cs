using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.TrainingBattle.State
{
    public class WonBattleStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private TrainingBattleStateController _stateController;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Show();
            _stateController.Input.SubmitEvent += ExitState;
            _stateController.Input.CancelEvent += ExitState;
            _stateController.Input.EnableInput();
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ExitState;
            _stateController.Input.CancelEvent -= ExitState;
        }

        private void ExitState()
        {
            if(!_stateController.IsExitState) return;
            _stateController.DialogController.Dialogue.Hide();
            _stateController.ExitStateEvent?.Invoke();
            _stateController.IsExitState = false;
        }
    }
}