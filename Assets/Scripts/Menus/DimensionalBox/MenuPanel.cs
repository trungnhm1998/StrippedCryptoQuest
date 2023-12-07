using CryptoQuest.Input;
using CryptoQuest.Menus.DimensionalBox.States;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class MenuPanel : UIMenuPanelBase
    {
        [field: SerializeField] public GameObject MagicStoneTransferPanel { get; private set; }
        [field: SerializeField] public TransferMagicStonesPanel TransferMagicStonesPanel { get; set; }
        [field: SerializeField] public TransferEquipmentsPanel EquipmentsTransferPanel { get; private set; }
        [field: SerializeField] public GameObject MetaDTransferPanel { get; private set; }
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
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
            Input.MenuNavigateEvent -= _stateMachine.Navigate;
            Input.MenuCancelEvent -= _stateMachine.Cancel;
            Input.MenuExecuteEvent -= _stateMachine.Execute;
            Input.MenuResetEvent -= _stateMachine.Reset;
        }

        public void ChangeState(int state) => _stateMachine.RequestStateChange((EState)state);
    }
}