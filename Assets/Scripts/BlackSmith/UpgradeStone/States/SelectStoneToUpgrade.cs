using CryptoQuest.Item.MagicStone;
using UnityEngine.EventSystems;

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
            if (_upgradableStoneListUI.Content.childCount > 0)
                EventSystem.current.SetSelectedGameObject(_upgradableStoneListUI.Content.GetChild(0).gameObject);
        }


        public override void OnCancel()
        {
            _stateMachine.BackToOverview();
        }
    }
}