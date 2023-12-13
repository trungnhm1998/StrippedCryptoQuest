using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class SelectMaterialForUpgrade : UpgradeMagicStoneStateBase
    {
        public SelectMaterialForUpgrade(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }
        private MagicStoneInventory _magicStoneInventory => _stateMachine.UpgradeMagicStoneSystem.MagicStoneInventory;

        public override void OnEnter()
        {
            base.OnEnter();
            var upgradableStones = _stateMachine.UpgradeMagicStoneSystem.GetUpgradableStones();
            _materialStoneList.RenderStones(upgradableStones);
            _materialStoneList.ClearStonesWithException(_stateMachine.StoneToUpgrade);
        }

        public override void OnExit()
        {
            base.OnExit();
            _materialStoneList.ClearStonesWithException();
            _materialStoneList.ResetMaterials();
        }


        public override void OnCancel()
        {
            _stateMachine.StoneToUpgrade = null;
            _materialStoneList.ClearStonesWithException();
            _stateMachine.RequestStateChange(EUpgradeMagicStoneStates.SelectStone);
        }
    }
}