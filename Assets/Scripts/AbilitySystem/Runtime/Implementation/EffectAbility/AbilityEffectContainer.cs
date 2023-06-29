using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    [System.Serializable]
    public class AbilityEffectContainer
    {
        public TargetType TargetType;
        public EffectScriptableObject[] Effects;
    }

    public class AbilityEffectContainerSpec
    {
        public List<AbstractEffect> EffectSpecs = new List<AbstractEffect>();
        public List<AbilitySystemBehaviour> Targets = new List<AbilitySystemBehaviour>();

        public void AddTargets(List<AbilitySystemBehaviour> targets)
        {
            if (targets.Count == 0) return;
            Targets.AddRange(targets);
        }
    }
}