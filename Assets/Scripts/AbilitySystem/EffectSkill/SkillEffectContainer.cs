using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Indigames.AbilitySystem
{
    [System.Serializable]
    public class SkillEffectContainer
    {
        public TargetType TargetType;
        public EffectScriptableObject[] Effects;
    }

    public class SkillEffectContainerSpec
    {
        public List<AbstractEffect> EffectSpecs = new List<AbstractEffect>();
        public List<SkillSystem> Targets = new List<SkillSystem>();

        public void AddTargets(List<SkillSystem> targets)
        {
            if (targets.Count == 0) return;
            Targets.AddRange(targets);
        }
    }
}