using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ExitStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Show();
            _stateController.Input.SubmitEvent += ExitState;
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ExitState;
        }

        private void ExitState()
        {
            if(!_stateController.IsExitState) return;
            _stateController.DialogController.Dialogue.Hide();
            _stateController.ExitStateEvent?.Invoke();
        }
    }
}
