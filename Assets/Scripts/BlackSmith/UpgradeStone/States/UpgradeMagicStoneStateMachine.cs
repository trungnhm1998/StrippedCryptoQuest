using CryptoQuest.BlackSmith.UpgradeStone.UI;
using CryptoQuest.Item.MagicStone;
using FSM;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public enum EUpgradeMagicStoneStates
    {
        SelectStone,
        SelectMaterialStone,
        ConfirmUpgrade
    }

    public class UpgradeMagicStoneStateMachine : StateMachine<string, EUpgradeMagicStoneStates, string>
    {
        public UIUpgradableStoneList UpgradableStoneListUI => UpgradeMagicStoneSystem.UpgradableStoneListUI;
        public UIMaterialStoneList MaterialStoneListUI => UpgradeMagicStoneSystem.MaterialStoneListUI;
        private readonly BlackSmithSystem _context;
        public UpgradeMagicStoneSystem UpgradeMagicStoneSystem { get; }
        private UpgradeMagicStoneStateBase _state;
        public UIUpgradableStone StoneToUpgrade { get; set; }

        public UpgradeMagicStoneStateMachine(BlackSmithSystem context)
        {
            _context = context;
            UpgradeMagicStoneSystem = context.UpgradeMagicStoneSystem;
            AddState(EUpgradeMagicStoneStates.SelectStone, new SelectStoneToUpgrade(this));
            AddState(EUpgradeMagicStoneStates.SelectMaterialStone, new SelectMaterialForUpgrade(this));
            SetStartState(EUpgradeMagicStoneStates.SelectStone);
        }

        public override void OnEnter()
        {
            UpgradeMagicStoneSystem.gameObject.SetActive(true);
            _context.Input.CancelEvent += OnCancel;
            base.OnEnter();
        }

        public void SetCurrentState(UpgradeMagicStoneStateBase state) => _state = state;

        public override void OnExit()
        {
            UpgradeMagicStoneSystem.gameObject.SetActive(false);
            _context.Input.CancelEvent -= OnCancel;
            base.OnExit();
        }

        private void OnCancel() => _state?.OnCancel();

        public void BackToOverview() => fsm.RequestStateChange(State.OVERVIEW);
    }
}