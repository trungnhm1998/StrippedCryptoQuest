using CryptoQuest.Input;
using CryptoQuest.Menus.DimensionalBox.States;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class MenuPanel : UIMenuPanelBase
    {
        [field: SerializeField] public GameObject MagicStoneTransferPanel { get; private set; }
        [field: SerializeField] public TransferMagicStonesPanel TransferMagicStonesPanel { get; set; }
        [field: SerializeField] public TransferEquipmentsPanel EquipmentsTransferPanel { get; private set; }
        [field: SerializeField] public MetadTransferPanel MetadTransferPanel { get; private set; }
        [field: SerializeField] public GameObject OverviewPanel { get; private set; }
        [SerializeField] private InputMediatorSO _input;
        public InputMediatorSO Input => _input;

        private DBoxStateMachine _stateMachine;

        private void Awake() => _stateMachine = new(this);

        private void OnEnable()
        {
            _stateMachine.Init();

            Input.MenuNavigateEvent += _stateMachine.Navigate;
            Input.MenuCancelEvent += _stateMachine.Cancel;
            Input.MenuExecuteEvent += _stateMachine.Execute;
            Input.MenuResetEvent += _stateMachine.Reset;
            Input.MenuInteractEvent += _stateMachine.Interact;
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
            Input.MenuNavigateEvent -= _stateMachine.Navigate;
            Input.MenuCancelEvent -= _stateMachine.Cancel;
            Input.MenuExecuteEvent -= _stateMachine.Execute;
            Input.MenuResetEvent -= _stateMachine.Reset;
            Input.MenuInteractEvent -= _stateMachine.Interact;
        }

        public void ChangeState(int state) => _stateMachine.RequestStateChange((EState)state);
    }
}