using CryptoQuest.UI.Menu.MenuStates.SkillStates;
using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class SkillMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavSkill = "NavSkill";
        public static readonly string Skill = "Skill";
        public static readonly string CharacterSelection = "CharacterSelection";
        public static readonly string SkillSelection = "SkillSelection";

        /// <summary>
        /// Setup the state machine for Skill menu.
        /// </summary>
        /// <param name="panel"></param>
        public SkillMenuStateMachine(UISkillMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavSkill, new GenericUnfocusState(CharacterSelection));
            AddState(Skill, new FocusSkillState(panel));
            AddState(CharacterSelection, new CharacterSelectionState(panel));
            AddState(SkillSelection, new SkillSelectionState(panel));

            SetStartState(CharacterSelection);
        }
    }
}