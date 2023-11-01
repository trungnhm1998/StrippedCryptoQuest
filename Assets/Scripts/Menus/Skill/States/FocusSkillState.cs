using CryptoQuest.Menus.Skill.UI;

namespace CryptoQuest.Menus.Skill.States
{
    public class FocusSkillState : SkillStateBase
    {
        public FocusSkillState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }
    }
}