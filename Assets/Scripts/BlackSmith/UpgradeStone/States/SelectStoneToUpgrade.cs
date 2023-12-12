using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class SelectStoneToUpgrade : UpgradeMagicStoneStateBase
    {
        public SelectStoneToUpgrade(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }
        private MagicStoneInventory _magicStoneInventory => _stateMachine.UpgradeMagicStoneSystem.MagicStoneInventory;

        public override void OnEnter()
        {
            base.OnEnter();
            _upgradableStoneListUI.ClearStonesWithException();
            _upgradableStoneListUI.RenderStones(_magicStoneInventory.MagicStones);
            _upgradableStoneListUI.StoneSelected += OnSelectBaseItem;
        }

        public override void OnExit()
        {
            base.OnExit();
            _upgradableStoneListUI.StoneSelected -= OnSelectBaseItem;
            _upgradableStoneListUI.ClearStonesWithException();
        }


        public override void OnCancel()
        {
            _stateMachine.BackToOverview();
        }

        private void OnSelectBaseItem(UIUpgradableStone stone)
        {
            _stateMachine.StoneToUpgrade = stone;
            _stateMachine.RequestStateChange(EUpgradeMagicStoneStates.SelectMaterialStone);
        }
    }
}