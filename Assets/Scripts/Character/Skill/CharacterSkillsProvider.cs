using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class CharacterSkillsProvider : MonoBehaviour, ISkillsProvider
    {
        [SerializeField] private SkillDatabase _database;
        [SerializeField] private HeroSkillSetSO _heroSkillSetSO;

        private void Awake()
        {
            ServiceProvider.Provide<ISkillsProvider>(this);
        }

        public void GetSkills(HeroBehaviour hero, Action<List<CastSkillAbility>> skillsLoadedCallback)
        {
            hero.TryGetComponent(out Battle.Components.LevelSystem levelSystem);
            hero.TryGetComponent(out Element elementComp);

            var selectedSkills = (from skillMapping in _heroSkillSetSO.SkillMappings
                where skillMapping.Class == hero.Class.Id
                      && skillMapping.Element == elementComp.ElementValue.Id
                      && skillMapping.Level <= levelSystem.Level
                select skillMapping).ToList();

            if (selectedSkills.Count > 0) StartCoroutine(CoLoadSkills(selectedSkills, skillsLoadedCallback));
        }

        private IEnumerator CoLoadSkills(List<HeroSkillsSet> skillMappings,
            Action<List<CastSkillAbility>> skillsLoadedCallback)
        {
            var skills = new List<CastSkillAbility>();
            foreach (var skillMapping in skillMappings)
            {
                Debug.Log($"Load skill {skillMapping.Skill}");
                yield return _database.LoadDataById(skillMapping.Skill);
                if (_database.TryGetDataById(skillMapping.Skill, out var skill) && skill != null)
                    skills.Add(skill);
            }

            skillsLoadedCallback?.Invoke(skills);
        }
    }
}