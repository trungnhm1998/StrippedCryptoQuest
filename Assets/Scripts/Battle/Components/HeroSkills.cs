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
        [field: SerializeField] public List<CastSkillAbility> Skills { get; private set; } = new();

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
            Skills.AddRange(skills);
            Skills = Skills.Distinct().ToList();
        }
    }
}