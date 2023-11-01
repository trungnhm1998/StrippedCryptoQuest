using CryptoQuest.Menus.Skill.UI;

namespace CryptoQuest.Menus.Skill.States
{
    public class SkillSelectionState : SkillStateBase
    {
        public SkillSelectionState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO);
            SkillPanel.SkillListPanel.Init();
            UsingSkillPresenter.EnterTargetSingleCharacter += ChangeToTargetCharacterState;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }

        public override void OnExit()
        {
            UsingSkillPresenter.EnterTargetSingleCharacter -= ChangeToTargetCharacterState;
        }

        private void ChangeToTargetCharacterState()
        {
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.TargetSingleCharacter);
        }    
    }
}