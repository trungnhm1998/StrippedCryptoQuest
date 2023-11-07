using CryptoQuest.Input;
using CryptoQuest.Menus.DimensionalBox.States;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.UI
{
    internal class DimensionalBoxStateMachine : MonoBehaviour
    {
        [SerializeField] private GameObject _equipmentsTransferPanel;
        public GameObject EquipmentsTransferPanel => _equipmentsTransferPanel;
        [SerializeField] private GameObject _metaDTransferPanel;
        public GameObject MetaDTransferPanel => _metaDTransferPanel;
        [SerializeField] private UILandingPage _landingPage;
        [SerializeField] private InputMediatorSO _input;
        public InputMediatorSO Input => _input;

        public StateBase Landing { get; private set; }
        public StateBase TransferringMetaDState { get; private set; }
        public StateBase TransferringEquipmentsState { get; private set; }
        public StateBase ConfirmTransfer { get; set; }

        private StateBase _currentState;

        private void Awake()
        {
            Landing = new LandingPage(_landingPage);
            TransferringEquipmentsState = new TransferringEquipments(_equipmentsTransferPanel);
            TransferringMetaDState = new TransferringMetaD(_metaDTransferPanel);
            ConfirmTransfer = new ConfirmTransfer();
        }

        private void OnEnable()
        {
            ChangeState(Landing);
        }

        private void OnDisable()
        {
            _currentState?.Exit();
        }

        public void ChangeState(StateBase newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(this);
        }
    }
}