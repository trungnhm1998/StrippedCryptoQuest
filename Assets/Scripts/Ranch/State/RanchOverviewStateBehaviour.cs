using UnityEngine;

namespace CryptoQuest.Ranch.State
{
    public class RanchOverviewStateBehaviour : BaseStateBehaviour
    {
        private static readonly int SwapState = Animator.StringToHash("SwapState");
        private static readonly int UpgradeState = Animator.StringToHash("UpgradeState");
        private static readonly int EvolveState = Animator.StringToHash("EvolveState");

        protected override void OnEnter()
        {
            _stateController.OpenSwapEvent += OpenSwapState;
            _stateController.OpenUpgradeEvent += OpenUpgradeState;
            _stateController.OpenEvolveEvent += OpenEvolveState;

            _stateController.DialogController.RanchOpened();

            _input.CancelEvent += ExitState;
        }

        protected override void OnExit()
        {
            _stateController.OpenSwapEvent -= OpenSwapState;
            _stateController.OpenUpgradeEvent -= OpenUpgradeState;
            _stateController.OpenEvolveEvent -= OpenEvolveState;

            _stateController.DialogController.NormalDialogue.Hide();

            _input.CancelEvent -= ExitState;
        }

        private void OpenSwapState() => StateMachine.Play(SwapState);
        private void OpenUpgradeState() => StateMachine.Play(UpgradeState);
        private void OpenEvolveState() => StateMachine.Play(EvolveState);
        private void ExitState() => _stateController.ExitStateEvent?.Invoke();
    }
}