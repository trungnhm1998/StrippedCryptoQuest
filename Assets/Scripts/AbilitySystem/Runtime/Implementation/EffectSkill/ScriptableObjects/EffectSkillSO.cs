using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "EffectSkill", menuName = "Indigames Ability System/Skills/Effect Skill")]
    public class EffectSkillSO : SkillScriptableObject<EffectSkill>
    {
        /// <summary>
        /// Effect applied using tag in desired timing (eg. PostAttack,...)
        /// Or assign a Tag named OnActive to apply the effect when activate skill
        /// </summary>
        /// <typeparam name="EffectTagMap"></typeparam>
        /// <returns></returns>
        public List<EffectTagMap> EffectContainerMap = new();
        
        [Serializable]
        public class EffectTagMap
        {
            public TagScriptableObject Tag;
            public List<SkillEffectContainer> TargetContainer = new List<SkillEffectContainer>();
        }
    }

    public class EffectSkillSO<T> : EffectSkillSO where T : EffectSkill, new()
    {
        protected override AbstractSkill CreateSkill() => new T();
    }
}