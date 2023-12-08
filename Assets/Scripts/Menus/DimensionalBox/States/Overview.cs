using CryptoQuest.UI.Menu;
using UI.Common;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.States
{
    internal class Overview : DBoxStateBase
    {
        private Button[] _buttons;

        public Overview(MenuPanel menuPanel) : base(menuPanel)
        {
            AddAction(EStateAction.OnCancel, BackToNavigation);
        }

        public override void OnEnter()
        {
            MenuPanel.EquipmentsTransferPanel.gameObject.SetActive(false);
            MenuPanel.MetaDTransferPanel.SetActive(false);
            MenuPanel.MagicStoneTransferPanel.SetActive(false);

            MenuPanel.OverviewPanel.gameObject.SetActive(true);
            MenuPanel.Focusing += SelectDefaultButton;

            SelectDefaultButton();
        }

        public override void OnExit()
        {
            MenuPanel.Focusing -= SelectDefaultButton;
            MenuPanel.OverviewPanel.gameObject.SetActive(false);
        }

        private void SelectDefaultButton()
        {
            EnableButtons(true);
            MenuPanel.OverviewPanel.GetComponentInChildren<SelectFirstChildInList>().Select();
        }

        private void BackToNavigation()
        {
            EnableButtons(false);
            UIMainMenu.OnBackToNavigation();
        }

        private void EnableButtons(bool enabled)
        {
            _buttons ??= MenuPanel.GetComponentsInChildren<Button>();
            foreach (var button in _buttons) button.interactable = enabled;
        }
    }
}