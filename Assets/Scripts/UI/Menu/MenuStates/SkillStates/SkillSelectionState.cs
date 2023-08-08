using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class SkillSelectionState : SkillStateBase
    {
        private UISkillManager _skillSelectionOverview;

        public SkillSelectionState(UISkillMenu skillPanel) : base(skillPanel)
        {
            _skillSelectionOverview = skillPanel.SkillSelectionOverview;
        }
        
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
            _skillSelectionOverview.OnTurnBack();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }

        public override void OnExit()
        {
            base.OnExit();
            _skillSelectionOverview.OnTurnBack();
        }
    }
}