using CryptoQuest.Menus.Skill.UI;
using CryptoQuest.UI.Menu.MenuStates;

namespace CryptoQuest.Menus.Skill.States
{
    public class SkillMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavSkill = "NavSkill";
        public static readonly string CharacterSelection = "CharacterSelection";
        public static readonly string SkillSelection = "SkillSelection";
        public static readonly string TargetSingleCharacter = "TargetSingleCharacter";

        /// <summary>
        /// Setup the state machine for Skill menu.
        /// </summary>
        /// <param name="panel"></param>
        public SkillMenuStateMachine(UISkillMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavSkill, new GenericUnfocusState(CharacterSelection));
            AddState(CharacterSelection, new CharacterSelectionState(panel));
            AddState(SkillSelection, new SkillSelectionState(panel));
            AddState(TargetSingleCharacter, new TargetSingleCharacterState(panel));

            SetStartState(CharacterSelection);
        }
    }
}