using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class EffectCustomApplicationRequirement : ScriptableObject
    {
        public abstract bool CanApplyEffect(EffectScriptableObject effect, AbstractEffect effectSpec,
            AbilitySystem ownerSystem);
    }
}