using CryptoQuest.Input;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using FSM;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public enum EMetadState
    {
        FetchToken = 0,
        SelectSource = 1,
        InputTransferAmount = 2,
        ConfirmTransfer = 3,
    }

    public class TransferMetadStateMachine : StateMachine<EState, EMetadState, EStateAction>
    {
        private DBoxStateMachine _rootFsm;
        public InputMediatorSO Input => _rootFsm.Panel.Input;
        public MetadTransferPanel Panel => _rootFsm.Panel.MetadTransferPanel;
        public CurrencySO SelectedCurrency => Panel.SelectedCurrency;
        public float TransferAmount { get; set; }

        public TransferMetadStateMachine(DBoxStateMachine rootFsm) : base(false)
        {
            _rootFsm = rootFsm;

            AddState(EMetadState.FetchToken, new FetchToken(this));
            AddState(EMetadState.SelectSource, new SelectSource(this));
            AddState(EMetadState.InputTransferAmount, new InputTransferAmount(this));
            AddState(EMetadState.ConfirmTransfer, new ConfirmTransfer(this));

            SetStartState(EMetadState.FetchToken);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Panel.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Panel.gameObject.SetActive(false);
        }

        public void BackToOverview() => fsm.RequestStateChange(EState.Overview);
    }
}