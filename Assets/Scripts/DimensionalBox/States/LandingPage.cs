using CryptoQuest.DimensionalBox.UI;
using UnityEngine.EventSystems;

namespace CryptoQuest.DimensionalBox.States
{
    internal class LandingPage : StateBase
    {
        private readonly UILandingPage _landingPage;

        public LandingPage(UILandingPage landingPage)
        {
            _landingPage = landingPage;
        }

        protected override void OnEnter()
        {
            _landingPage.gameObject.SetActive(true);
            _landingPage.TransferringEquipments += ChangeToTransferEquipmentState;
            _landingPage.TransferringMetaD += ChangeToTransferMetaDState;
            
            EventSystem.current.SetSelectedGameObject(_landingPage.DefaultSelectedButton);
        }

        protected override void OnExit()
        {
            _landingPage.TransferringEquipments -= ChangeToTransferEquipmentState;
            _landingPage.TransferringMetaD -= ChangeToTransferMetaDState;
            _landingPage.gameObject.SetActive(false);
        }

        private void ChangeToTransferMetaDState() => StateMachine.ChangeState(StateMachine.TransferringMetaDState);

        private void ChangeToTransferEquipmentState() => StateMachine.ChangeState(StateMachine.TransferringEquipmentsState);
    }
}