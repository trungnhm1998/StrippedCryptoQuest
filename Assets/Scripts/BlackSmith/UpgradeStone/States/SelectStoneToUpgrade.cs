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
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.SelectStoneToUpdateText).Show();

            _stoneUpgradePresenter.gameObject.SetActive(true);
            _materialStonesPresenter.ClearStones();
            _upgradableStonePresenter.ClearStones();
            _upgradableStonePresenter.RenderStones(distinctStoneType);
            _upgradableStonePresenter.StoneSelected += OnSelectBaseItem;
        }

        public override void OnExit()
        {
            base.OnExit();
            _dialogsPresenter.Dialogue.Hide();
            _upgradableStonePresenter.StoneSelected -= OnSelectBaseItem;
            _upgradableStonePresenter.ClearStones();
        }

        private List<IMagicStone> GetDistinctStoneType(List<IMagicStone> stones)
        {
            return stones.GroupBy(stone => new { stone.Definition, stone.Level })
                .Select(group => group.First())
                .ToList();
        }


        public override void OnCancel()
        {
            _stateMachine.BackToOverview();
        }

        private void OnSelectBaseItem(UIUpgradableStone stone)
        {
            _stateMachine.StoneToUpgrade = stone;
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectMaterialStone);
        }
    }
}