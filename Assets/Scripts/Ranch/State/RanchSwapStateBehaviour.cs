using UnityEngine;

namespace CryptoQuest.Ranch.State
{
    public class RanchSwapStateBehaviour : BaseStateBehaviour
    {
        private RanchStateController _stateController;
        private static readonly int OverView = Animator.StringToHash("OverviewState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _stateController.RanchController.Input.CancelEvent += ExitState;
        }

        protected override void OnExit()
        {
            _stateController.RanchController.Input.CancelEvent -= ExitState;
        }

        private void ExitState()
        {
            StateMachine.Play(OverView);
            _stateController.RanchController.Initialize();
        }
    }
}