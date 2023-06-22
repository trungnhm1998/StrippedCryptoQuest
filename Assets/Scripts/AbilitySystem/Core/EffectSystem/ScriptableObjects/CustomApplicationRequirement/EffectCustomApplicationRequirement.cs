using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class EffectCustomApplicationRequirement : ScriptableObject
    {
        public abstract bool CanApplyGameplayEffect(EffectScriptableObject effect, AbstractEffect effectSpec,
            SkillSystem system);
    }
}