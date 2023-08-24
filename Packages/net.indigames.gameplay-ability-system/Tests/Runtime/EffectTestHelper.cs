using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.CustomApplicationRequirement;

namespace IndiGames.GameplayAbilitySystem.Tests.Runtime
{
    public static class EffectTestHelper
    {
        public static void SetupEffectSO(AttributeScriptableObject attribute,
            EffectScriptableObject effectSO,
            EAttributeModifierType modifierType = EAttributeModifierType.Add,
            float value = 1)
        {
            effectSO.EffectDetails = new EffectDetails();
            effectSO.EffectDetails.Modifiers = new EffectAttributeModifier[] {
                new EffectAttributeModifier()
                {
                    Attribute = attribute,
                    ModifierType = modifierType,
                    ModifierMagnitude = null,
                    Value = value
                }
            };
            effectSO.EffectDetails.StackingType = EModifierType.External;
        }
    }

    public class MockParameters : AbilityParameters {}
    
    public class FalseCustomRequirement : EffectCustomApplicationRequirement 
    {
        public override bool CanApplyEffect(EffectScriptableObject effect, GameplayEffectSpec effectSpecSpec, AbilitySystemBehaviour ownerSystem)
        {
            return false;
        }
    }
    
    public class TrueCustomRequirement : EffectCustomApplicationRequirement 
    {
        public override bool CanApplyEffect(EffectScriptableObject effect, GameplayEffectSpec effectSpecSpec, AbilitySystemBehaviour ownerSystem)
        {
            return true;
        }
    }

}