using CryptoQuest.Ranch.Sagas;
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
        [SerializeField] private LocalizedString _failedMessage;

        private TinyMessageSubscriptionToken _upgradeSucceed;
        private TinyMessageSubscriptionToken _upgradeFailed;
        private TinyMessageSubscriptionToken _getDataSucceed;

        protected override void OnEnter()
        {
            _upgradeSucceed = ActionDispatcher.Bind<BeastUpgradeSucceed>(OnUpgradeSucceed);
            _upgradeFailed = ActionDispatcher.Bind<BeastUpgradeFailed>(OnUpgradeFailed);
            _getDataSucceed = ActionDispatcher.Bind<GetBeastSucceeded>(OnGetDataSuccess);

            _config.InitUI();

            _input.NavigateEvent += InputOnNavigateEvent;
            _input.CancelEvent += BackToUpgradeState;

            _stateController.DialogController.ChoiceDialog
                .SetMessage(_warningMessage)
                .WithYesCallback(OnYesButtonClicked)
                .WithNoCallback(BackToUpgradeState)
                .Show();
        }

        private void OnUpgradeFailed(ActionBase _)
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            _stateController.DialogController.NormalDialogue
                .SetMessage(_failedMessage)
                .Show();

            _input.SubmitEvent += BackToUpgradeState;
        }

        private void OnUpgradeSucceed(ActionBase _)
        {
            _config.UpdateGold();
            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }

        private void OnGetDataSuccess(ActionBase _) => StateMachine.Play(UpgradeResultState);

        private void InputOnNavigateEvent(Vector2 handleNavigation) => _config.HandleNavigation(handleNavigation);

        private void BackToUpgradeState() => StateMachine.Play(UpgradeState);

        private void OnYesButtonClicked()
        {
            if (!_config.IsUpgradeValid)
            {
                _config.DeInitUI();

                _stateController.DialogController.ChoiceDialog.Hide();
                _stateController.DialogController.NormalDialogue
                    .SetMessage(_failedMessage)
                    .Show();

                _input.SubmitEvent += BackToUpgradeState;

                return;
            }

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
            ActionDispatcher.Unbind(_getDataSucceed);

            _config.DeInitUI();
            _stateController.DialogController.ChoiceDialog.Hide();

            _input.CancelEvent -= BackToUpgradeState;
            _input.SubmitEvent -= BackToUpgradeState;
            _input.NavigateEvent -= InputOnNavigateEvent;
        }
    }
}