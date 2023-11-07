using CryptoQuest.Menus.DimensionalBox.UI;
using CryptoQuest.UI.Menu;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.States
{
    internal class LandingPage : StateBase
    {
        private readonly UILandingPage _landingPage;
        private Button[] _buttons;

        public LandingPage(UILandingPage landingPage)
        {
            _landingPage = landingPage;
        }

        protected override void OnEnter()
        {
            StateMachine.EquipmentsTransferPanel.SetActive(false);
            StateMachine.MetaDTransferPanel.SetActive(false);
            _landingPage.gameObject.SetActive(true);
            StateMachine.Input.MenuCancelEvent += OnBackToNavigation;
            _landingPage.TransferringEquipments += ChangeToTransferEquipmentState;
            _landingPage.TransferringMetaD += ChangeToTransferMetaDState;
            StateMachine.Focusing += SelectDefaultButton;

            SelectDefaultButton();
        }

        protected override void OnExit()
        {
            StateMachine.Input.MenuCancelEvent -= OnBackToNavigation;
            _landingPage.TransferringEquipments -= ChangeToTransferEquipmentState;
            _landingPage.TransferringMetaD -= ChangeToTransferMetaDState;
            StateMachine.Focusing -= SelectDefaultButton;
            _landingPage.gameObject.SetActive(false);
        }


        private void SelectDefaultButton()
        {
            EnableButtons(true);
            StateMachine.SelectDefault();
        }

        private void OnBackToNavigation()
        {
            EnableButtons(false);
            UIMainMenu.OnBackToNavigation();
        }

        private void EnableButtons(bool enabled)
        {
            _buttons ??= _landingPage.GetComponentsInChildren<Button>();
            foreach (var button in _buttons) button.interactable = enabled;
        }

        private void ChangeToTransferMetaDState() => StateMachine.ChangeState(StateMachine.TransferringMetaDState);

        private void ChangeToTransferEquipmentState() =>
            StateMachine.ChangeState(StateMachine.TransferringEquipmentsState);
    }
}