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

            UIBeast.OnBeastSelected += EquipBeast;

            ShowBeastList();
            _beastPanel.ListBeastUI.Interactable = true;
        }

        public override void OnExit()
        {
            _beastPanel.ListBeastUI.Interactable = false;
            _beastPanel.DetailBeastUI.SetEnabled(false);

            _beastPanel.Input.MenuCancelEvent -= HandleCancel;
            _beastPanel.Input.MenuNavigateEvent -= NavigateSelector;
            UIBeast.OnBeastSelected -= EquipBeast;
            _beastPanel.Focusing -= ShowBeastList;
        }

        private void EquipBeast(UIBeast ui)
        {
            _beastPanel.ListBeastUI.EquipBeast(ui);
        }

        private void NavigateSelector(Vector2 dir) => _beastPanel.ListBeastUI.DisplayNavigateArrows();

        private void ShowBeastList()
        {
            _beastPanel.DetailBeastUI.SetEnabled(_beastPanel.ListBeastUI.IsValid);
            _beastPanel.ListBeastUI.Init();
            _beastPanel.ListBeastUI.SelectFirstBeast();
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }
    }
}