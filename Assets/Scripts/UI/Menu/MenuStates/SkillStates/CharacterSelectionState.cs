using CryptoQuest.UI.Menu.Panels.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    public class CharacterSelectionState : SkillStateBase
    {
        private UISkillManager _characterSelectionOverview;

        public CharacterSelectionState(UISkillMenu skillPanel) : base(skillPanel)
        {
            _characterSelectionOverview = skillPanel.CharacterSelectionOverview;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO);
            // _characterSelectionOverview.EquipmentSlotSelected += ChangeEquipment;
            _characterSelectionOverview.InitCharacterSelection();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(SkillPanel.TypeSO, true);
            // _equipmentOverviewPanel.DeInit();
            MenuStateMachine.RequestStateChange(SkillMenuStateMachine.NavSkill);
        }

        public override void OnExit()
        {
            base.OnExit();
            // _equipmentOverviewPanel.DeInit();
            // _equipmentOverviewPanel.EquipmentSlotSelected -= ChangeEquipment;
        }
    }
}