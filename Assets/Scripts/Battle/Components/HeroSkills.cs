using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Character;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroSkills : CharacterComponentBase
    {
        [SerializeField] private List<CastSkillAbility> _skills;
        public IReadOnlyList<CastSkillAbility> Skills => _skills;

        private ISkillsProvider _skillsProvider;
        private ISkillsProvider Provider => _skillsProvider ??= ServiceProvider.GetService<ISkillsProvider>();

        /// <summary>
        /// Based on character level and class, get all skills that character can use
        /// </summary>
        public override void Init()
        {
            Provider.GetSkills(Character.GetComponent<HeroBehaviour>(), AddSkillsToHero);
        }

        private void AddSkillsToHero(List<CastSkillAbility> skills)
        {
            _skills.AddRange(skills);
            _skills = _skills.Distinct().ToList();
        }
    }
}