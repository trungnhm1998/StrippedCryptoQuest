using CryptoQuest.BlackSmith.UpgradeStone.UI;
using FSM;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public enum EUpgradeMagicStoneStates
    {
        SelectStone,
        SelectUpgradableStone,
        ConfirmUpgrade
    }

    public class UpgradeMagicStoneStateMachine : StateMachine<string, EUpgradeMagicStoneStates, string>
    {
        public UIUpgradableStoneList UpgradableStoneListUI => UpgradeMagicStoneSystem.UpgradableStoneListUI;
        private readonly BlackSmithSystem _context;
        public UpgradeMagicStoneSystem UpgradeMagicStoneSystem { get; }
        private UpgradeMagicStoneStateBase _state;

        public UpgradeMagicStoneStateMachine(BlackSmithSystem context)
        {
            _context = context;
            UpgradeMagicStoneSystem = context.UpgradeMagicStoneSystem;
            AddState(EUpgradeMagicStoneStates.SelectStone, new SelectStoneToUpgrade(this));
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