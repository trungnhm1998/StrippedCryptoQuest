using CryptoQuest.Input;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeResultState : BaseStateBehaviour
    {
        private RanchStateController _stateController;
        private MerchantsInputManager _input;

        private static readonly int UpgradeState = Animator.StringToHash("UpgradeState");

        private TinyMessageSubscriptionToken _getDataSucceed;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _input = _stateController.Controller.Input;

            _getDataSucceed = ActionDispatcher.Bind<GetNftBeastsSucceed>(InitResult);

            _input.CancelEvent += BackToUpgradeState;
            _input.SubmitEvent += BackToUpgradeState;

            ActionDispatcher.Dispatch(new GetBeasts());
        }

        private void InitResult(ActionBase _)
        {
            var beast = _stateController.UpgradePresenter.BeastToUpgrade;

            var firstBeast = _stateController.BeastInventory.OwnedBeasts.Find(x => x.BeastId == beast.BeastId);

            _stateController.UpgradePresenter.InitBeast(_stateController.BeastInventory.OwnedBeasts);
            _stateController.UpgradePresenter.ResultBeast.Show(firstBeast);
        }

        private void BackToUpgradeState() => StateMachine.Play(UpgradeState);

        protected override void OnExit()
        {
            ActionDispatcher.Unbind(_getDataSucceed);

            _stateController.UpgradePresenter.ResultBeast.Hide();

            _input.CancelEvent -= BackToUpgradeState;
            _input.SubmitEvent -= BackToUpgradeState;
        }
    }
}