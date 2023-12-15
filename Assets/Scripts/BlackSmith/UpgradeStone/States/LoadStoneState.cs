using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class LoadStoneState : UpgradeMagicStoneStateBase
    {
        public LoadStoneState(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }
        private TinyMessageSubscriptionToken _fetchStoneSuccessToken;
        private TinyMessageSubscriptionToken _fetchStoneFailedToken;

        public override void OnEnter()
        {
            base.OnEnter();
            _stateMachine.UpgradeMagicStoneSystem.StoneList.Clear();
            _materialStonesPresenter.ClearStones();
            _upgradableStonePresenter.ClearStones();
            _stoneUpgradePresenter.gameObject.SetActive(true);
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.SelectMaterialText).Show();
            _magicStoneTooltip.gameObject.SetActive(false);
            _currencyPresenter.Hide();

            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
            _fetchStoneSuccessToken = ActionDispatcher.Bind<ResponseGetMagicStonesSucceeded>(HandleFetchStoneSuccess);
            _fetchStoneFailedToken = ActionDispatcher.Bind<ResponseGetMagicStonesFailed>(HandleFetchStoneFailed);
        }

        public override void OnExit()
        {
            base.OnExit();
            ActionDispatcher.Unbind(_fetchStoneSuccessToken);
            ActionDispatcher.Unbind(_fetchStoneFailedToken);
        }

        private void HandleFetchStoneFailed(ResponseGetMagicStonesFailed obj)
        {
            _stateMachine.BackToOverview();
        }

        private void HandleFetchStoneSuccess(ResponseGetMagicStonesSucceeded ctx)
        {
            _stateMachine.UpgradeMagicStoneSystem.StoneList = new(ctx.Stones);
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectStone);
        }
    }
}