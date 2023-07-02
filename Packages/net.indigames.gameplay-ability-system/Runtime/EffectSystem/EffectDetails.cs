using System;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    [Serializable]
    public struct EffectDetails
    {
        public EffectAttributeModifier[] Modifiers;
        public EEffectStackingType StackingType;
    }

    public enum EEffectStackingType
    {
        External,
        Core,
    }
}