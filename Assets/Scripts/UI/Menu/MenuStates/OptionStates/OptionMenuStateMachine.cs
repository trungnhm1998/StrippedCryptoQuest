using CryptoQuest.UI.Menu.Panels.Option;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class OptionMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavOption = "NavOption";
        public static readonly string Option = "Option";
        
        private new readonly UIOptionMenu _panel;
        
        /// <summary>
        /// Setup the state machine for option menu.
        /// </summary>
        /// <param name="panel"></param>
        public OptionMenuStateMachine(UIOptionMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavOption, new UnFocusOptionState(panel));
            AddState(Option, new FocusOptionState(panel));

            SetStartState(Option);
        }
    }
}