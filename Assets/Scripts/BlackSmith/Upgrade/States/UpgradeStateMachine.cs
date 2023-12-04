using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using FSM;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public enum EStates
    {
        SelectEquipment = 0,
        ConfigUpgrade = 1,
        ConfirmUpgrade = 2,
        UpgradeResult = 3,
    }

    public class UpgradeStateMachine : StateMachine<string, EStates, string>
    {
        private readonly BlackSmithSystem _context;
        private UpgradeStateBase _state;
        public UpgradeSystem UpgradeSystem { get; }
        public BlackSmithDialogsPresenter DialogsPresenter => _context.DialogPresenter;
        public MerchantsInputManager Input => _context.Input;

        public IEquipment EquipmentToUpgrade { get; set; }
        public float GoldNeeded { get; set; }
        public int LevelToUpgrade { get; set; }

        public UpgradeStateMachine(BlackSmithSystem context) : base(false)
        {
            _context = context;
            UpgradeSystem = _context.UpgradeSystem;

            AddState(EStates.SelectEquipment, new SelectUpgradeEquipmentState(this));
            AddState(EStates.ConfigUpgrade, new ConfigUpgradeState(this));
            AddState(EStates.ConfirmUpgrade, new ConfirmUpgradeState(this));
            AddState(EStates.UpgradeResult, new UpgradeResultState(this));

            SetStartState(EStates.SelectEquipment);
        }

        public void SetCurrentState(UpgradeStateBase state) => _state = state;

        public override void OnEnter()
        {
            UpgradeSystem.gameObject.SetActive(true);
            Input.CancelEvent += OnCancel;
            Input.SubmitEvent += OnSubmit;
            base.OnEnter();
        }

        public override void OnExit()
        {
            UpgradeSystem.gameObject.SetActive(false);
            Input.CancelEvent -= OnCancel;
            Input.SubmitEvent -= OnSubmit;
            base.OnExit();
        }

        private void OnCancel() => _state?.OnCancel();

        private void OnSubmit() => _state?.OnSubmit();

        public void BackToOverview() => fsm.RequestStateChange(State.OVERVIEW);
    }
}