using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menus.Skill.UI;
using CryptoQuest.UI.Menu;
using IndiGames.Core.Common;
using UnityEngine;

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
            _skillPanel.Input.MenuNavigateEvent += ShowHeroSkills;
            
            SelectFirstHero();
        }

        public override void OnExit()
        {
            foreach (var heroButton in _skillPanel.HeroButtons) heroButton.Selecting -= CacheLastSelectingSlot;

            _skillPanel.Input.MenuCancelEvent -= HandleCancel;
            _skillPanel.Focusing -= SelectFirstHero;
            _skillPanel.Input.MenuConfirmedEvent -= ToSelectSkillState;
            _skillPanel.Input.MenuNavigateEvent -= ShowHeroSkills;

            DeSelectAllHeroes();
        }

        private void ShowHeroSkills(Vector2 direction)
        {
            if (_skillPanel.SelectingHero == null) return;
            _skillPanel.SkillListPanel.TryShowSkillForHero(_skillPanel.SelectingHero.Hero);
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
            _skillPanel.SkillListPanel.TryShowSkillForHero(ServiceProvider.GetService<IPartyController>().Slots[0].HeroBehaviour);
            _skillPanel.EnableAllHeroButtons();
            DeActiveSelectedHero();

            var selectButton = (_skillPanel.SelectingHero != null) ?
                _skillPanel.SelectingHero : _skillPanel.HeroButtons[0];

            selectButton.Select();
            selectButton.EnableSelectBackground();
            _skillPanel.SelectingHero = selectButton;
        }
    }
}