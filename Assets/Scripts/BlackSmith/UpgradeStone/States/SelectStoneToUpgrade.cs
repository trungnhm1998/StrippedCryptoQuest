using System.Collections.Generic;
using System.Linq;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class SelectStoneToUpgrade : UpgradeMagicStoneStateBase
    {
        public SelectStoneToUpgrade(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            var upgradableStones = _stateMachine.UpgradeMagicStoneSystem.GetUpgradableStones();
            var distinctStoneType = GetDistinctStoneType(upgradableStones);
            _upgradableStoneListUI.ClearStonesWithException();
            _upgradableStoneListUI.RenderStones(distinctStoneType);
            _upgradableStoneListUI.StoneSelected += OnSelectBaseItem;
        }

        public override void OnExit()
        {
            base.OnExit();
            _upgradableStoneListUI.StoneSelected -= OnSelectBaseItem;
            _upgradableStoneListUI.ClearStonesWithException();
        }

        private List<IMagicStone> GetDistinctStoneType(List<IMagicStone> stones)
        {
            var distinctStoneType = new List<IMagicStone>();
            foreach (var stone in stones)
            {
                if (distinctStoneType.Any(x => x.ID == stone.ID)) continue;
                distinctStoneType.Add(stone);
            }

            return distinctStoneType;
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