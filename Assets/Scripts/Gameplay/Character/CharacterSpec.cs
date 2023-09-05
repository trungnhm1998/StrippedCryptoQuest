using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character.LevelSystem;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.UI.Menu.Panels.Home;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Use this to save game
    /// </summary>
    [Serializable]
    public class CharacterSpec
    {

        [field: SerializeField] public CharacterBackgroundInfo BackgroundInfo { get; private set; }
        [field: SerializeField] public CharacterClass Class { get; private set; }
        [field: SerializeField] public Elemental Element { get; set; }
        [field: SerializeField] public StatsDef StatsDef { get; set; }
        [field: SerializeField] public CharacterEquipments Equipments { get; private set; }
        [field: SerializeField] public float Experience { get; set; }

        // I use static here so we only have to create level calculator once
        // Also call .Level first to make sure that calculator being created
        public static ILevelCalculator LevelCalculator { get; private set; }
        public int Level 
        {
            get 
            {
                LevelCalculator ??= new LevelCalculator(StatsDef.MaxLevel);
                return LevelCalculator.CalculateCurrentLevel(Experience);
            }
        }

        // TODO: #1136 Remove serialize and load runtime using Class, Element info
        [field: SerializeField] private CharacterSkillSet _skillSet;
        public CharacterSkillSet SkillSet => _skillSet;

        // TODO: #1136 Remove serialize and load runtime using Class, BaseInfo info
        [field: SerializeField] private Sprite _avatar;
        public Sprite Avatar => _avatar;

        private CharacterBehaviourBase _characterComponent;
        public CharacterBehaviourBase CharacterComponent => _characterComponent;

        public bool IsValid()
        {
            return BackgroundInfo != null
                   && Element != null
                   && Class != null
                   && StatsDef.Attributes.Length > 0;
        }

        public List<AbilityData> GetAvailableSkills()
        {
            if (_skillSet == null) return new();
            return _skillSet.GetSkillsByCurrentLevel(Level);
        }

        // TODO: Implement this
        public GameplayAbilitySpec CreateSkillSpec(AbilityData data) => new();

        public void Init(CharacterBehaviourBase characterBehaviour)
        {
            Bind(characterBehaviour);
            Equipments.ClearEventRegistration();
        }

        public void Bind(CharacterBehaviourBase characterBehaviour)
        {
            _characterComponent = characterBehaviour;
        }

        public void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetElement(Element.Icon);
            uiCharacterInfo.SetLevel(Level);
            uiCharacterInfo.SetClass(Class.Name);
            uiCharacterInfo.SetAvatar(_avatar);
            BackgroundInfo.SetupUI(uiCharacterInfo);
        }
    }
}