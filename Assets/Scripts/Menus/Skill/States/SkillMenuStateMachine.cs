using CryptoQuest.Menus.Skill.UI;
using FSM;

namespace CryptoQuest.Menus.Skill.States
{
    public class SkillMenuStateMachine : StateMachine
    {
        public static readonly string CharacterSelection = "CharacterSelection";
        public static readonly string SkillSelection = "SkillSelection";

        /// <summary>
        /// Setup the state machine for Skill menu.
        /// </summary>
        /// <param name="panel"></param>
        public SkillMenuStateMachine(UISkillMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(CharacterSelection, new CharacterSelectionState(panel));
            AddState(SkillSelection, new SkillSelectionState(panel));

            SetStartState(CharacterSelection);
        }
    }
}