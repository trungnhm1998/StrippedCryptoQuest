using CryptoQuest.Menus.Skill.UI;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Menu.Character;

namespace CryptoQuest.Menus.Skill.States
{
    public class CharacterSelectionState : SkillStateBase
    {
        public CharacterSelectionState(UISkillMenu skillPanel) : base(skillPanel) { }

        private void DeSelectAllHeroes() => _skillPanel.EnableAllHeroButtons(false);
        private void DeActiveSelectedHero(bool active = false) => _skillPanel.EnableHeroSelectedMode(active);

        public override void OnEnter()
        {
            foreach (var heroButton in _skillPanel.HeroButtons) heroButton.Selecting += CacheLastSelectingSlot;

            DeSelectAllHeroes();

            _skillPanel.Input.MenuCancelEvent += HandleCancel;
            _skillPanel.Focusing += SelectFirstHero;
            _skillPanel.Input.MenuConfirmedEvent += ToSelectSkillState;
            
            SelectFirstHero();
        }

        public override void OnExit()
        {
            foreach (var heroButton in _skillPanel.HeroButtons) heroButton.Selecting -= CacheLastSelectingSlot;

            _skillPanel.Input.MenuCancelEvent -= HandleCancel;
            _skillPanel.Focusing -= SelectFirstHero;
            _skillPanel.Input.MenuConfirmedEvent -= ToSelectSkillState;

            DeSelectAllHeroes();
        }

        private void ToSelectSkillState()
        {
            if (_skillPanel.SelectingHero == null) return;
            _skillPanel.EnableAllHeroSelecting(false);
            fsm.RequestStateChange(SkillMenuStateMachine.SkillSelection);
        }

        private void CacheLastSelectingSlot(UISkillCharacterPartySlot hero) => _skillPanel.SelectingHero = hero;

        private void HandleCancel()
        {
            _skillPanel.SelectingHero = null;
            UIMainMenu.OnBackToNavigation();
        }

        private void SelectFirstHero()
        {
            _skillPanel.EnableAllHeroButtons();
            DeActiveSelectedHero();

            if (_skillPanel.SelectingHero != null)
            {
                _skillPanel.SelectingHero.Select();
                _skillPanel.SelectingHero.EnableSelectBackground();
                return;
            }

            _skillPanel.HeroButtons[0].Select();
            _skillPanel.HeroButtons[0].EnableSelectBackground();
        }
    }
}