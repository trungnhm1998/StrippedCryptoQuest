using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    [Serializable]
    public class EffectDetails
    {
        public EffectAttributeModifier[] Modifiers = Array.Empty<EffectAttributeModifier>();
        public EModifierType StackingType = EModifierType.Core;
    }
}