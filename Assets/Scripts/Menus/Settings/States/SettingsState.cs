using CryptoQuest.Menus.Settings.UI;
using CryptoQuest.UI.Menu;
using FSM;

namespace CryptoQuest.Menus.Settings.States
{
    public class SettingsState : StateBase
    {
        protected UISettingsMenu _settingsPanel;

        public SettingsState(UISettingsMenu panel) : base(false)
        {
            _settingsPanel = panel;
        }

        public override void OnEnter()
        {
            UIMainMenu.OnBackToNavigation();
            _settingsPanel.Input.MenuCancelEvent += HandleCancel;
            _settingsPanel.LanguageOptions.Initialize();
        }

        public override void OnExit()
        {
            _settingsPanel.Input.MenuCancelEvent -= HandleCancel;
            _settingsPanel.LanguageOptions.DeInitialize();
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }
    }
}
