using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class OverviewStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += ChangeState;
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
        }

        private void ChangeState()
        {
            StateMachine.Play(SelectCharacter);
        }
    }
}
