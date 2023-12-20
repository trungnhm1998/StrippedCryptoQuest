using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveResultState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _evolveSucceededMessage;
        [SerializeField] private LocalizedString _evolveFailedMessage;
        [SerializeField] private LocalizedString _evolveSuccessTitle;
        [SerializeField] private LocalizedString _evolveFailTitle;
        private RanchStateController _controller;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += ChangeConfirmState;
            UpdateResultStats();
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.SubmitEvent -= ChangeConfirmState;
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
            _controller.DialogController.NormalDialogue.Hide();
        }

        private void UpdateResultStats()
        {
            var isSuccess = _controller.EvolveStatus;
            var message = isSuccess ? _evolveSucceededMessage : _evolveFailedMessage;
            _controller.DialogController.NormalDialogue.SetMessage(message).Show();
            
            var title = isSuccess ? _evolveSuccessTitle : _evolveFailTitle;
            int beastId = _controller.BeastEvolveId;
            _controller.EvolvePresenter.ShowBeastResult(title, beastId);
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