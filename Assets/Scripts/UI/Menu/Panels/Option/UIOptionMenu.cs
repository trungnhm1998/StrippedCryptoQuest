using CryptoQuest.System.Settings;
using CryptoQuest.UI.Menu.MenuStates.OptionStates;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Option
{
    public class UIOptionMenu : UIMenuPanel
    {
        [field: SerializeField] public LanguageSettingController LanguageSettingController { get; private set; }

        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new OptionMenuStateMachine(this);
        }
    }
}