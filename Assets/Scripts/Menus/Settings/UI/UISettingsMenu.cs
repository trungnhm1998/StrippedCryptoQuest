using CryptoQuest.Input;
using CryptoQuest.Menus.Settings.States;
using CryptoQuest.System.Settings;
using CryptoQuest.UI.Menu;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.Settings.UI
{
    public class UISettingsMenu : UIMenuPanelBase
    {
        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        [field: SerializeField] public UILanguageOptions LanguageOptions { get; private set; }
        [field: SerializeField] public UISettingSlider VolumeSlider { get; private set; }
        [field: SerializeField] public UILogoutButton LogoutButton { get; private set; }

        private StateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = new SettingsMenuStateMachine(this);
        }

        private void OnEnable()
        {
            _stateMachine.Init();
        }

        private void OnDisable()
        {
            _stateMachine.OnExit();
        }
    }
}