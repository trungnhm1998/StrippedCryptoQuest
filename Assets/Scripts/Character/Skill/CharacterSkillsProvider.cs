using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Character
{
    public interface ISkillsProvider
    {
        void GetSkills(HeroBehaviour hero, Action<List<CastableAbility>> skillsLoadedCallback);
    }

    public class CharacterSkillsProvider : MonoBehaviour, ISkillsProvider
    {
        [Serializable]
        public struct SkillMapping
        {
            public int SkillId;
            public int Level;
            public int ClassId;
            public int ElementId;
        }

        [SerializeField] private SkillDatabase _database;
        [SerializeField] private List<SkillMapping> _skillMappings = new();

        private void Awake()
        {
            ServiceProvider.Provide<ISkillsProvider>(this);
        }

        public void GetSkills(HeroBehaviour hero, Action<List<CastableAbility>> skillsLoadedCallback)
        {
            hero.TryGetComponent(out Battle.Components.LevelSystem levelSystem);
            hero.TryGetComponent(out Element elementComp);

            var selectedSkills = (from skillMapping in _skillMappings
                where skillMapping.ClassId == hero.Class.Id
                      && skillMapping.ElementId == elementComp.ElementValue.Id
                      && skillMapping.Level <= levelSystem.Level
                select skillMapping).ToList();

            if (selectedSkills.Count > 0) StartCoroutine(CoLoadSkills(selectedSkills, skillsLoadedCallback));
        }

        private IEnumerator CoLoadSkills(List<SkillMapping> skillMappings,
            Action<List<CastableAbility>> skillsLoadedCallback)
        {
            var skills = new List<CastableAbility>();
            foreach (var skillMapping in skillMappings)
            {
                Debug.Log($"Load skill {skillMapping.SkillId}");
                yield return _database.LoadDataById(skillMapping.SkillId);
                if (_database.TryGetDataById(skillMapping.SkillId, out var skill))
                    skills.Add(skill);
            }

            skillsLoadedCallback?.Invoke(skills);
        }
    }
}