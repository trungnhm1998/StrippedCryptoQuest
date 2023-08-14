using CryptoQuest.UI.Menu.Panels.Option;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class OptionMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavOption = "NavOption";
        public static readonly string Option = "Option";
        public static readonly string Language = "Language";

        /// <summary>
        /// Setup the state machine for option menu.
        /// </summary>
        /// <param name="panel"></param>
        public OptionMenuStateMachine(UIOptionMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavOption, new GenericUnfocusState(Language));
            AddState(Option, new FocusOptionState(panel));
            AddState(Language, new LanguageState(panel));

            SetStartState(Language);
        }
    }
}