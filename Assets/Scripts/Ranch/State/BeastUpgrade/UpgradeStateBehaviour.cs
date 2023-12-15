using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _welcomeMessage;
        [SerializeField] private LocalizedString _overviewMessage;
        private RanchStateController _controller;

        private static readonly int OverviewState = Animator.StringToHash("OverviewState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastUpgrade.Contents.SetActive(true);

            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.DialogManager.NormalDialogue.SetMessage(_welcomeMessage).Show();

            _controller.Controller.ShowWalletEventChannel.EnableAll().Show();
        }

        private void CancelBeastEvolveState()
        {
            _controller.DialogManager.NormalDialogue.SetMessage(_overviewMessage).Show();
            _controller.UIBeastUpgrade.Contents.SetActive(false);
            _controller.Controller.Initialize();
            StateMachine.Play(OverviewState);
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
            _controller.Controller.ShowWalletEventChannel.Hide();
        }
    }
}