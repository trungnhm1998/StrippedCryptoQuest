using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ConfirmStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int Result = Animator.StringToHash("ResultState");
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += ChangeState;
            _stateController.Input.CancelEvent += ExitState;
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
            _stateController.Input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            StateMachine.Play(Result);
        }

        private void ExitState()
        {
            StateMachine.Play(SelectCharacter);
        }
    }
}
