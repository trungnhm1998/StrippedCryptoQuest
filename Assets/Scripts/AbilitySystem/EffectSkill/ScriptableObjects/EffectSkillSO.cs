using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "EffectSkill", menuName = "Indigames Ability System/Skills/Effect Skill")]
    public class EffectSkillSO : SkillScriptableObject<EffectSkill>
    {
        [Serializable]
        public class EffectTagMap
        {
            public TagScriptableObject Tag;
            public List<SkillEffectContainer> TargetContainer = new List<SkillEffectContainer>();
        }

        public List<EffectTagMap> EffectContainerMap = new List<EffectTagMap>();
    }

    public class EffectSkillSO<T> : EffectSkillSO where T : EffectSkill, new()
    {
        protected override AbstractSkill CreateSkill() => new T();
    }
}