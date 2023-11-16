using UnityEngine;

namespace CryptoQuest.Ranch.State
{
    public class RanchOverviewStateBehaviour : BaseStateBehaviour
    {
        private RanchStateController _stateController;

        private static readonly int SwapState = Animator.StringToHash("SwapState");
        private static readonly int UpgradeState = Animator.StringToHash("UpgradeState");
        private static readonly int EvolveState = Animator.StringToHash("EvolveState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();

            _stateController.OpenSwapEvent += OpenSwapState;
            _stateController.OpenUpgradeEvent += OpenUpgradeState;
            _stateController.OpenEvolveEvent += OpenEvolveState;

            _stateController.RanchController.Input.CancelEvent += ExitState;
        }

        protected override void OnExit()
        {
            _stateController.OpenSwapEvent -= OpenSwapState;
            _stateController.OpenUpgradeEvent -= OpenUpgradeState;
            _stateController.OpenEvolveEvent -= OpenEvolveState;

            _stateController.RanchController.Input.CancelEvent -= ExitState;
        }

        private void OpenSwapState()
        {
            StateMachine.Play(SwapState);
            _stateController.RanchController.HideDialogs();
        }

        private void OpenUpgradeState()
        {
            StateMachine.Play(UpgradeState);
            _stateController.RanchController.HideDialogs();
        }

        private void OpenEvolveState()
        {
            StateMachine.Play(EvolveState);
            _stateController.RanchController.HideDialogs();
        }

        private void ExitState()
        {
            _stateController.RanchController.HideDialogs();
            _stateController.ExitStateEvent?.Invoke();
        }
    }
}