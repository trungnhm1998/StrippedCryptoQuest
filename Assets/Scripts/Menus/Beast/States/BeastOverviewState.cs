using CryptoQuest.Menus.Beast.UI;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.States
{
    public class BeastOverviewState : BeastStateBase
    {
        public BeastOverviewState(UIBeastMenu beastMenuPanel) : base(beastMenuPanel) { }

        public override void OnEnter()
        {
            _beastPanel.Input.MenuCancelEvent += HandleCancel;
            _beastPanel.Input.MenuNavigateEvent += NavigateSelector;
            _beastPanel.Focusing += ShowBeastList;

            ShowBeastList();
        }

        public override void OnExit()
        {
            _beastPanel.Input.MenuCancelEvent -= HandleCancel;
            _beastPanel.Input.MenuNavigateEvent -= NavigateSelector;
            _beastPanel.Focusing -= ShowBeastList;
        }

        private void NavigateSelector(Vector2 dir) => _beastPanel.ListBeastUI.DisplayNavigateArrows();

        private void ShowBeastList()
        {
            _beastPanel.ListBeastUI.ShowBeast();
            _beastPanel.ListBeastUI.Interactable = true;
            _beastPanel.ListBeastUI.SelectLastedOrFirstBeast();
        }

        private void HandleCancel()
        {
            _beastPanel.ListBeastUI.Interactable = false;
            UIMainMenu.OnBackToNavigation();
        }
    }
}