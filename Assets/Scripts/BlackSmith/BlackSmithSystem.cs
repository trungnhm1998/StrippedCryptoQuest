using CryptoQuest.BlackSmith.Evolve.UI;
using CryptoQuest.BlackSmith.States.Overview;
using CryptoQuest.BlackSmith.Upgrade;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Input;
using CryptoQuest.Merchant;
using UnityEngine;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithSystem : MerchantSystemBase
    {
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public UIBlackSmithOverview OverviewUI { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }
        [field: SerializeField] public UpgradeSystem UpgradeSystem { get; private set; }
        [field: SerializeField] public EvolveSystem EvolveSystem { get; private set; }
        [field: SerializeField] public UpgradeMagicStoneSystem UpgradeMagicStoneSystem { get; private set; }
        private BlackSmithStateMachine _stateMachine;

        protected override void OnInit()
        {
            _stateMachine ??= new(this);
            _stateMachine.Init();
        }

        protected override void OnExit()
        {
            _stateMachine?.OnExit();
        }
    }
}