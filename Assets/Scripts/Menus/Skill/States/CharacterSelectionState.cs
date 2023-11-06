using CryptoQuest.Menus.Skill.UI;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Menu.Character;

namespace CryptoQuest.Menus.Skill.States
{
    public class CharacterSelectionState : SkillStateBase
    {
        public CharacterSelectionState(UISkillMenu skillPanel) : base(skillPanel) { }

        public override void OnEnter()
        {
            DeselectAllHeroes();
            foreach (var heroButton in SkillPanel.HeroButtons) heroButton.Selecting += CacheLastSelectingSlot;
            SkillPanel.Input.MenuCancelEvent += HandleCancel;
            SkillPanel.Focusing += SelectFirstHero;
            SkillPanel.Input.MenuConfirmedEvent += ToSelectSkillState;

            SelectFirstHero();
        }


        public override void OnExit()
        {
            foreach (var heroButton in SkillPanel.HeroButtons) heroButton.Selecting -= CacheLastSelectingSlot;
            SkillPanel.Input.MenuCancelEvent -= HandleCancel;
            SkillPanel.Focusing -= SelectFirstHero;
            SkillPanel.Input.MenuConfirmedEvent -= ToSelectSkillState;
            SkillPanel.EnableAllHeroButtons(false);
        }

        private void ToSelectSkillState()
        {
            if (SkillPanel.SelectingHero == null) return;
            SkillPanel.SelectingHero.IsSelected = true;
            fsm.RequestStateChange(SkillMenuStateMachine.SkillSelection);
        }

        private void CacheLastSelectingSlot(UICharacterPartySlot hero) => SkillPanel.SelectingHero = hero;

        private void DeselectAllHeroes()
        {
            foreach (var button in SkillPanel.HeroButtons) button.IsSelected = false;
        }

        private void HandleCancel()
        {
            SkillPanel.SelectingHero = null;
            SkillPanel.EnableAllHeroButtons(false);
            UIMainMenu.OnBackToNavigation();
        }

        private void SelectFirstHero()
        {
            SkillPanel.EnableAllHeroButtons();
            if (SkillPanel.SelectingHero != null)
            {
                SkillPanel.SelectingHero.Select();
                return;
            }

            SkillPanel.HeroButtons[0].Select();
        }
    }
}