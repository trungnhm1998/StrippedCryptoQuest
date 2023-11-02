using CryptoQuest.Menus.Skill.UI;

namespace CryptoQuest.Menus.Skill.States
{
    public class FocusSkillState : SkillStateBase
    {
        public FocusSkillState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter() { }

        private void HandleCancel() { }

        private void Interact()
        {
            fsm.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }
    }
}