using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeStateBehaviour : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private static readonly int OverviewState = Animator.StringToHash("OverviewState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastEvolve.Contents.SetActive(true);

            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
        }

        private void CancelBeastEvolveState()
        {
            _controller.UIBeastEvolve.Contents.SetActive(false);
            _controller.Controller.Initialize();
            StateMachine.Play(OverviewState);
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
        }
    }
}