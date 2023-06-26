using System.Collections.Generic;

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
        public List<AbilitySystem> Targets = new List<AbilitySystem>();

        public void AddTargets(List<AbilitySystem> targets)
        {
            if (targets.Count == 0) return;
            Targets.AddRange(targets);
        }
    }
}