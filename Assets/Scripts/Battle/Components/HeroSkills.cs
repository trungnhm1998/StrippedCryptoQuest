using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character;
using CryptoQuest.Character.Ability;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroSkills : CharacterComponentBase
    {
        [SerializeField] private List<CastableAbility> _skills;
        public IReadOnlyList<CastableAbility> Skills => _skills;

        private ISkillsProvider _skillsProvider;
        private ISkillsProvider Provider => _skillsProvider ??= ServiceProvider.GetService<ISkillsProvider>();

        /// <summary>
        /// Based on character level and class, get all skills that character can use
        /// </summary>
        public override void Init()
        {
            Provider.GetSkills(GetComponent<HeroBehaviour>(), AddSkillsToHero);
        }

        private void AddSkillsToHero(List<CastableAbility> skills)
        {
            _skills.AddRange(skills);
            _skills = _skills.Distinct().ToList();
        }
    }
}