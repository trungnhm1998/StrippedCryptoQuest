using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
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
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }
    }
}