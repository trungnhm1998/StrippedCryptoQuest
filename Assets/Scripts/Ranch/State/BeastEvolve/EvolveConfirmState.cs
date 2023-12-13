using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Ranch.Sagas;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveConfirmState : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");
        private static readonly int ResultState = Animator.StringToHash("EvolveResultState");

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
        
        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new RequestEvolveBeast()
            {
                Base =  _controller.EvolvePresenter.BeastToEvolve,
                Material = _controller.EvolvePresenter.BeastMaterial
            });
        }

        private void ChangeConfirmState()
        {
            StateMachine.Play(ResultState);
            HandleConfirmEvolving();
        }

        private void CancelBeastEvolveState()
        {
            StateMachine.Play(EvolveState);
        }
    }
}