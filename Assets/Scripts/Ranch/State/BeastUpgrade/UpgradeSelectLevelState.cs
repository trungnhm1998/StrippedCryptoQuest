using CryptoQuest.Input;
using CryptoQuest.Ranch.Sagas;
using CryptoQuest.Ranch.Upgrade.UI;
using IndiGames.Core.Events;
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


        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _config = _stateController.UpgradePresenter.UIConfigBeastUpgradePresenter;
            _input = _stateController.Controller.Input;

            _stateController.DialogManager.ChoiceDialog
                .SetMessage(_warningMessage)
                .WithYesCallback(OnYesButtonClicked)
                .WithNoCallback(CancelUpgradeSelectLevelState)
                .Show();

            _config.InitUI();

            _input.NavigateEvent += InputOnNavigateEvent;
            _input.CancelEvent += CancelUpgradeSelectLevelState;
        }

        private void InputOnNavigateEvent(Vector2 handleNavigation)
        {
            _config.HandleNavigation(handleNavigation);
        }

        private void CancelUpgradeSelectLevelState()
        {
            StateMachine.Play(UpgradeState);
        }

        private void OnYesButtonClicked()
        {
            ActionDispatcher.Dispatch(new RequestUpgradeBeast()
            {
                BeforeLevel = _config.LevelToUpgrade,
                Beast = _stateController.UpgradePresenter.BeastToUpgrade
            });

            StateMachine.Play(UpgradeState);
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= CancelUpgradeSelectLevelState;
            _config.DeInitUI();

            _stateController.DialogManager.ChoiceDialog.Hide();
        }
    }
}