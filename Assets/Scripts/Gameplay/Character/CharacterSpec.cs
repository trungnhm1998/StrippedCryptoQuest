using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.UI.Menu.Panels.Home;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using CryptoQuest.Gameplay.Character.LevelSystem;
using UnityEngine.Localization;
using Equipments = CryptoQuest.Character.Hero.Equipments;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Use this to save game
    /// </summary>
    [Serializable]
    public class CharacterSpec
    {
        [field: SerializeField] public Origin BackgroundInfo { get; private set; }
        [field: SerializeField] public CharacterClass Class { get; private set; }
        [field: SerializeField] public Elemental Element { get; set; }
        [field: SerializeField] public StatsDef StatsDef { get; set; }
        [field: SerializeField] public Equipments Equipments { get; private set; }
        [field: SerializeField] public float Experience { get; set; }

        [field: SerializeField, ReadOnly] public int Level { get; set; }

        [field: SerializeField, ReadOnly] public CharacterSkillSet SkillSet { get; set; }
        [field: SerializeField] public Sprite Avatar { get; set; }
        [field: SerializeField] public LocalizedString Name { get; private set; }

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
            if (SkillSet == null) return new();
            return SkillSet.GetSkillsByCurrentLevel(Level);
        }

        public void Init(CharacterBehaviourBase characterBehaviour)
        {
            Bind(characterBehaviour);
        }

        public void Bind(CharacterBehaviourBase characterBehaviour)
        {
            _characterComponent = characterBehaviour;
        }
    }
}