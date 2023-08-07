using CryptoQuest.UI.Menu.MenuStates.SkillStates;
using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class SkillMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavStatus = "NavStatus";

        private new readonly UISkillMenu _panel;
        
        /// <summary>
        /// Setup the state machine for status menu.
        /// </summary>
        /// <param name="panel"></param>
        public SkillMenuStateMachine(UISkillMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavStatus, new UnFocusSkillState(panel));
            // AddState(Status, new FocusStatusState(panel));
            // AddState(Equipment, new EquipmentState(panel));
            // AddState(EquipmentSelection, new ChangeEquipmentState(panel));
            //
            // SetStartState(Equipment);
        }
    }
}