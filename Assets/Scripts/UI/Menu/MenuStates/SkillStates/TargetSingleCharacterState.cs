using CryptoQuest.Battle.Components;
using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class TargetSingleCharacterState : SkillStateBase
    {
        public TargetSingleCharacterState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            SkillPanel.TargetSingleCharacterUI.ShowUI();
            SkillPanel.TargetSingleCharacterUI.SelectedCharacterEvent += SelectedCharacter;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.SkillSelection);
        }

        public override void OnExit()
        {
            base.OnExit();
            SkillPanel.TargetSingleCharacterUI.SetActiveUI(false);
            SkillPanel.TargetSingleCharacterUI.SelectedCharacterEvent -= SelectedCharacter;
        }

        private void SelectedCharacter(HeroBehaviour hero)
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.SkillSelection);
        }
    }
}