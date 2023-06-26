using System;

namespace Indigames.AbilitySystem
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