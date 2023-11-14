using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ResultStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int Overview = Animator.StringToHash("OverviewState");

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
            StateMachine.Play(Overview);
        }
    }
}
