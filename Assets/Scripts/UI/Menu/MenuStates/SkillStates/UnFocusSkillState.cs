using CryptoQuest.UI.Menu.Panels.Skill;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class UnFocusSkillState : SkillStateBase
    {
        public UnFocusSkillState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO);
        }
        
        public override void Interact()
        {
            base.Interact();
            // MenuStateMachine.RequestStateChange(SkillMenuStateMachine.Equipment);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MainMenuContext.CloseMainMenu();
        }

        public override void ChangeTab(float direction)
        {
            base.ChangeTab(direction);
            NavigationBar.ChangeTab(direction);
        }
    }
}