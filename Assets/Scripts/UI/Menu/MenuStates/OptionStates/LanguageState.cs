using CryptoQuest.System.Settings;
using CryptoQuest.UI.Menu.Panels.Option;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class LanguageState : OptionStateBase
    {
        private UILanguageOptions _uiLanguageSettingOverviewPanel;

        public LanguageState(UIOptionMenu optionPanel) : base(optionPanel)
        {
            _uiLanguageSettingOverviewPanel = optionPanel.LanguageOptions;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO);
            
            _uiLanguageSettingOverviewPanel.Initialize();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO, true);
            
            _uiLanguageSettingOverviewPanel.DeInitialize();
            MenuStateMachine.RequestStateChange(OptionMenuStateMachine.NavOption);
        }
    }
}