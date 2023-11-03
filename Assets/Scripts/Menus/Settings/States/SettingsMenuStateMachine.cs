using CryptoQuest.Menus.Settings.UI;
using FSM;

namespace CryptoQuest.Menus.Settings.States
{
    public class SettingsMenuStateMachine : StateMachine
    {
        public static readonly string Settings = "Settings";

        private readonly UISettingsMenu _settingsMenuPanel;

        public SettingsMenuStateMachine(UISettingsMenu settingMenuPanel) : base(false)
        {
            _settingsMenuPanel = settingMenuPanel;

            AddState(Settings, new SettingsState(_settingsMenuPanel));

            SetStartState(Settings);
        }
    }
}
