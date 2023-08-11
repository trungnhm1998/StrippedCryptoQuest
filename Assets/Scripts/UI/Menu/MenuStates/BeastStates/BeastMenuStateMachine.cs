using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.Beast;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.BeastStates
{
    public class BeastMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavBeast = "NavBeast";
        public static readonly string Beast = "Beast";

        /// <summary>
        /// Setup the state machine for beast menu.
        /// </summary>
        /// <param name="panel"></param>
        public BeastMenuStateMachine(UIBeastMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavBeast, new GenericUnfocusState(Beast));
            AddState(Beast, new FocusBeastState(panel));

            SetStartState(Beast);
        }
    }
}