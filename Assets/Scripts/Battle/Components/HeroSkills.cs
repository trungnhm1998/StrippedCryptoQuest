using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Character;
using CryptoQuest.Character.Skill;
using CryptoQuest.System;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroSkills : CharacterComponentBase
    {
        [field: SerializeField] public List<CastSkillAbility> Skills { get; private set; } = new();

        private ISkillsProvider _skillsProvider;
        private ISkillsProvider Provider => _skillsProvider ??= ServiceProvider.GetService<ISkillsProvider>();

        private TinyMessageSubscriptionToken _levelUpToken; 
        private HeroBehaviour _hero;

        /// <summary>
        /// Based on character level and class, get all skills that character can use
        /// </summary>
        public override void Init()
        {
            _hero = Character.GetComponent<HeroBehaviour>();
            GetSkills();
        }

        private void GetSkills()
        {
            Skills.Clear();
            Provider.GetSkills(_hero, AddSkillsToHero);
        }

        private void OnEnable()
        {
            _levelUpToken = ActionDispatcher.Bind<HeroLeveledUpAction>(action => GetSkills());
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_levelUpToken);
        }
        
        private void AddSkillsToHero(List<CastSkillAbility> skills)
        {
            Skills.AddRange(skills);
            Skills = Skills.Distinct().ToList();
        }
    }
}