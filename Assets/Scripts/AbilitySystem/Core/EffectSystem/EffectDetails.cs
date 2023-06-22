using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [Serializable]
    public struct EffectDetails
    {
        public EffectAttributeModifier[] Modifiers;
        public EffectStackingType StackingType;
    }

    public enum EffectStackingType
    {
        External,
        Core,
    }
}