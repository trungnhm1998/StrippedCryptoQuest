using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    [Serializable]
    public struct EffectDetails
    {
        public EffectAttributeModifier[] Modifiers;
        public EModifierType StackingType;
    }
}