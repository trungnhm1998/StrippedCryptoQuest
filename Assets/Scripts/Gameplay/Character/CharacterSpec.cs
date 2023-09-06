using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.UI.Menu.Panels.Home;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using CryptoQuest.Gameplay.Character.LevelSystem;

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
        [field: SerializeField, ReadOnly] public int Level { get; set; }


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
            SetupExpUI(uiCharacterInfo);
        }

        private void SetupExpUI(ICharacterInfo uiCharacterInfo)
        {
            var levelCalculator = CharacterLevelComponent.LevelCalculator;
            if (levelCalculator == null) return;

            var cachedLevel = Level;
            var isMaxedLevel = cachedLevel == StatsDef.MaxLevel;
            var nextLevelRequireExp = levelCalculator.RequiredExps[isMaxedLevel ? cachedLevel - 1 : cachedLevel];
            uiCharacterInfo.SetMaxExp(nextLevelRequireExp);
            
            var currentLevelAccumulateExp = levelCalculator.AccumulatedExps[cachedLevel - 1];
            var currentExp = isMaxedLevel ? nextLevelRequireExp : Experience - currentLevelAccumulateExp;
            uiCharacterInfo.SetExp(currentExp);
        }
    }
}