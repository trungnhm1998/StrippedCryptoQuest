using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Beast;
using CryptoQuest.Input;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeResultState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _resultMessage;

        private RanchStateController _stateController;
        private MerchantsInputManager _input;

        private static readonly int UpgradeState = Animator.StringToHash("UpgradeState");

        private TinyMessageSubscriptionToken _getDataSucceed;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _input = _stateController.Controller.Input;
            _stateController.Controller.ShowWalletEventChannel
                .EnableSouls(false)
                .Show();

            _getDataSucceed = ActionDispatcher.Bind<GetBeastSucceeded>(InitResult);

            _input.CancelEvent += BackToUpgradeState;
            _input.SubmitEvent += BackToUpgradeState;

            _stateController.DialogController.NormalDialogue
                .SetMessage(_resultMessage)
                .Show();

            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }

        private void InitResult(ActionBase _)
        {
            IBeast beastToUpgrade = _stateController.UpgradePresenter.BeastToUpgrade;
            IBeast firstBeast = _stateController.BeastInventory.GetBeast(beastToUpgrade.Id);

            List<IBeast> ownedBeasts = _stateController.BeastInventory.OwnedBeasts;

            _stateController.UpgradePresenter.InitBeast(ownedBeasts);
            _stateController.UpgradePresenter.ResultBeast.Show(firstBeast);
            _stateController.UpgradePresenter.LeftPanel.SetActive(false);
            _stateController.UpgradePresenter.ActiveBeastDetail(false);
        }

        private void BackToUpgradeState() => StateMachine.Play(UpgradeState);

        protected override void OnExit()
        {
            ActionDispatcher.Unbind(_getDataSucceed);

            List<IBeast> ownedBeasts = _stateController.BeastInventory.OwnedBeasts;
            bool isValid = ownedBeasts.Any(beast => beast.Level < beast.MaxLevel);

            _stateController.UpgradePresenter.ActiveBeastDetail(isValid);
            _stateController.UpgradePresenter.ResultBeast.Hide();

            _input.CancelEvent -= BackToUpgradeState;
            _input.SubmitEvent -= BackToUpgradeState;
        }
    }
}