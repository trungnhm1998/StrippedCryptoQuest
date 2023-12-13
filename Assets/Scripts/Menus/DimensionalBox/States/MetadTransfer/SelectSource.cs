using CryptoQuest.Gameplay.Inventory.Currency;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class SelectSource : TransferMetadBaseState
    {
        public SelectSource(TransferMetadStateMachine fsm) : base(fsm)
        {
            AddAction(EStateAction.OnCancel, () => fsm.BackToOverview());
        }
    
        public override void OnEnter()
        {
            _panel.SetButtonsInteractable(true);
            _panel.SelectDefaultButton();
            _panel.SelectedCurrencySource += CurrencySelected;
        }
    
        public override void OnExit()
        {
            _panel.SetButtonsInteractable(false);
            _panel.SelectedCurrencySource -= CurrencySelected;
        }
    
        private void CurrencySelected(CurrencySO currency)
        {
            _fsm.RequestStateChange(EMetadState.InputTransferAmount);
        }
    }
}