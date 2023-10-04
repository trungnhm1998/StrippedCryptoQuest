using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character
{
    [CreateAssetMenu(fileName = "CharacterSkillSet", menuName = "Gameplay/Character/Skill Set")]
    public class CharacterSkillSet : ScriptableObject
    {
        [Serializable]
        public struct LevelSkillContainer
        {
            public int LevelRequire;
            public Gameplay.Skill.Skill[] Skills;
        }

        [SerializeField] private LevelSkillContainer[] _levelSkills;
        public LevelSkillContainer[] LevelSkills => _levelSkills;

        /// <summary>
        /// Get all skill in this skill set
        /// </summary>
        /// <value></value>
        public List<Gameplay.Skill.Skill> Skills
        {
            get
            {
                var allSkills = new List<Gameplay.Skill.Skill>();
                foreach (var levelSkill in _levelSkills)
                {
                    allSkills.AddRange(levelSkill.Skills);
                }
                return allSkills;
            }
        }

        public List<Gameplay.Skill.Skill> GetSkillsByCurrentLevel(int level)
        {
            var result = new List<Gameplay.Skill.Skill>();
            
            foreach (var skill in LevelSkills)
            {
                if (level >= skill.LevelRequire)
                {
                    result.AddRange(skill.Skills);
                }    
            }
            return result;
        }

#if UNITY_EDITOR
        /// <summary>
        /// For editor tool to import from master data
        /// </summary>
        /// <param name="levelSkills"></param>
        public void Editor_SetLevelSkills(LevelSkillContainer[] levelSkills)
        {
            _levelSkills = levelSkills;
        }
#endif
    }
}