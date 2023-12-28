using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class EntryState : UpgradeMagicStoneStateBase
    {
        public EntryState(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _materialStonesPresenter.ClearStones();
            _upgradableStonePresenter.ClearStones();
            _stoneUpgradePresenter.gameObject.SetActive(true);
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.SelectMaterialText).Show();
            _magicStoneTooltip.gameObject.SetActive(false);
            _currencyPresenter.Show();

            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectStone);
        }
    }
}