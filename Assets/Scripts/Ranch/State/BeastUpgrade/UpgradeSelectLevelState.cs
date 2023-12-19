using CryptoQuest.Input;
using CryptoQuest.Ranch.Sagas;
using CryptoQuest.Ranch.Upgrade.UI;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeSelectLevelState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _warningMessage;

        private RanchStateController _stateController;
        private UIConfigBeastUpgradePresenter _config;
        private MerchantsInputManager _input;

        private static readonly int UpgradeState = Animator.StringToHash("UpgradeState");
        private static readonly int ResultState = Animator.StringToHash("UpgradeResultState");

        private TinyMessageSubscriptionToken _upgradeSucceed;
        private TinyMessageSubscriptionToken _upgradeFailed;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _config = _stateController.UpgradePresenter.ConfigBeast;
            _input = _stateController.Controller.Input;

            _upgradeSucceed = ActionDispatcher.Bind<BeastUpgradeSucceed>(OnUpgradeSucceed);
            _upgradeFailed = ActionDispatcher.Bind<BeastUpgradeFailed>(OnUpgradeFailed);

            _config.InitUI();

            _input.NavigateEvent += InputOnNavigateEvent;
            _input.CancelEvent += BackToUpgradeState;

            _stateController.DialogController.ChoiceDialog
                .SetMessage(_warningMessage)
                .WithYesCallback(OnYesButtonClicked)
                .WithNoCallback(BackToUpgradeState)
                .Show();
        }

        private void OnUpgradeFailed(ActionBase _) => _input.SubmitEvent += BackToUpgradeState;

        private void OnUpgradeSucceed(ActionBase _) => StateMachine.Play(ResultState);

        private void InputOnNavigateEvent(Vector2 handleNavigation) => _config.HandleNavigation(handleNavigation);

        private void BackToUpgradeState() => StateMachine.Play(UpgradeState);

        private void OnYesButtonClicked()
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new RequestUpgradeBeast()
            {
                LevelToUpgrade = _config.LevelToUpgrade,
                Beast = _stateController.UpgradePresenter.BeastToUpgrade
            });
        }

        protected override void OnExit()
        {
            ActionDispatcher.Unbind(_upgradeSucceed);
            ActionDispatcher.Unbind(_upgradeFailed);

            _config.DeInitUI();
            _stateController.DialogController.ChoiceDialog.Hide();

            _input.CancelEvent -= BackToUpgradeState;
            _input.SubmitEvent -= BackToUpgradeState;
            _input.NavigateEvent -= InputOnNavigateEvent;
        }
    }
}