using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveResultState : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastEvolve.Contents.SetActive(true);
            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += ChangeConfirmState;
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.SubmitEvent -= ChangeConfirmState;
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
        }

        private void ChangeConfirmState()
        {
            StateMachine.Play(EvolveState);
        }

        private void CancelBeastEvolveState()
        {
            StateMachine.Play(EvolveState);
        }
    }
}