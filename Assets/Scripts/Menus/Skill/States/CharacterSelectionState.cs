using CryptoQuest.Menus.Skill.UI;

namespace CryptoQuest.Menus.Skill.States
{
    public class CharacterSelectionState : SkillStateBase
    {
        public CharacterSelectionState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO);
            SkillPanel.CharactersPanel.Init();
            SkillPanel.CharactersPanel.SelectedCharacterEvent += SelectedCharacter;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.NavSkill);
        }

        public override void OnExit()
        {
            base.OnExit();
            SkillPanel.CharactersPanel.SelectedCharacterEvent -= SelectedCharacter;
        }

        private void SelectedCharacter()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.SkillSelection);
        }
    }
}