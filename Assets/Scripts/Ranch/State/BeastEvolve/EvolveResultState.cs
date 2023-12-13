using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveResultState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _evolveSucceededMessage;
        [SerializeField] private LocalizedString _evolveFailedMessage;
        private TinyMessageSubscriptionToken _evolveResponseData;
        private RanchStateController _controller;
        private bool _isSucceeded;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            _controller.UIBeastEvolve.Contents.SetActive(true);
            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += ChangeConfirmState;
            SetMessage();
        }

        private void SetMessage()
        {
            LocalizedString message = _isSucceeded ? _evolveSucceededMessage : _evolveFailedMessage;
            _controller.DialogManager.NormalDialogue.SetMessage(message).Show();
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.SubmitEvent -= ChangeConfirmState;
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
            _controller.DialogManager.NormalDialogue.Hide();
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