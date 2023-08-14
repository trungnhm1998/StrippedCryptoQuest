using CryptoQuest.System.Settings;
using CryptoQuest.UI.Menu.Panels.Option;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class LanguageState : OptionStateBase
    {
        private LanguageController _uiLanguageOverviewPanel;

        public LanguageState(UIOptionMenu optionPanel) : base(optionPanel)
        {
            _uiLanguageOverviewPanel = optionPanel.LanguageController;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO);
            _uiLanguageOverviewPanel.Initialize();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO, true);
            _uiLanguageOverviewPanel.DeInitialize();
            MenuStateMachine.RequestStateChange(OptionMenuStateMachine.NavOption);
        }

        public override void OnExit()
        {
            base.OnExit();
            _uiLanguageOverviewPanel.DeInitialize();
        }
    }
}