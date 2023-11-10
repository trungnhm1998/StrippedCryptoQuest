using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Input;
using CryptoQuest.Menus.Skill.States;
using CryptoQuest.UI.Menu;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;

namespace CryptoQuest.Menus.Skill.UI
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Skill Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UISkillMenu : UIMenuPanelBase
    {
        [Header("State Context")]
        [SerializeField] private UICharacterSelection _charactersPanel;

        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        public UICharacterSelection CharactersPanel => _charactersPanel;

        [SerializeField] private UISkillList _skillListPanel;
        public UISkillList SkillListPanel => _skillListPanel;

        [field: SerializeField] public UISkillCharacterPartySlot[] HeroButtons { get; private set; }

        [field: SerializeField] public SkillTargetType SingleAlliedTarget { get; private set; }
        [field: SerializeField] public SkillTargetType AllAlliesTarget { get; private set; }
        [field: SerializeField] public SkillTargetType SelfTarget { get; private set; }

        public UISkillCharacterPartySlot SelectingHero { get; set; }

        private SkillMenuStateMachine _skillMenuStateMachine;

        private void Awake()
        {
            _skillMenuStateMachine = new SkillMenuStateMachine(this);
        }

        private void OnEnable()
        {
            foreach (var heroButton in HeroButtons) heroButton.EnableSelectBackground(false);
            _skillMenuStateMachine.Init();
        }

        private void OnDisable()
        {
            _skillMenuStateMachine.OnExit();
        }

        public void EnableAllHeroButtons(bool isEnabled = true)
        {
            foreach (var button in HeroButtons) button.Interactable = isEnabled;
        }

        public void EnableAllHeroSelecting(bool isEnabled = true)
        {
            foreach (var button in HeroButtons) button.IsSelected = isEnabled;
        }
        
        public void EnableHeroSelectedMode(bool isEnabled = true)
        {
            foreach (var button in HeroButtons) button.IsCharacterSelected = isEnabled;
        }
    }
}