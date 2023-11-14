using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class OverviewStateBehaviour : BaseStateBehaviour
    {
        private ChurchStateController _stateController;
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");
        private static readonly int ExitChurch = Animator.StringToHash("ExitState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.DialogController.YesPressedEvent += ChangeState;
            _stateController.DialogController.NoPressedEvent += ExitState;
            _stateController.IsExitState = false;
        }

        protected override void OnExit()
        {
            _stateController.DialogController.YesPressedEvent -= ChangeState;
            _stateController.DialogController.NoPressedEvent -= ExitState;
        }

        private void ChangeState()
        {
            StateMachine.Play(SelectCharacter);
        }

        private void ExitState()
        {
            StateMachine.Play(ExitChurch);
            _stateController.IsExitState = true;
        }
    }
}
