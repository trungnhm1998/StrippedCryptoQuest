using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
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
                    AttributeSO = attribute,
                    ModifierType = modifierType,
                    ModifierComputationMethod = null,
                    Value = value
                }
            };
            effectSO.EffectDetails.StackingType = EEffectStackingType.External;
        }

        public static AbstractEffect SetupAndApplyEffect(AbilitySystemBehaviour system, AttributeScriptableObject attribute,
            EffectScriptableObject effectSO,
            EAttributeModifierType modifierType = EAttributeModifierType.Add,
            float value = 1, float baseValue = 10)
        {
            system.AttributeSystem.AddAttributes(attribute);
            system.AttributeSystem.SetAttributeBaseValue(attribute, baseValue);
            SetupEffectSO(attribute, effectSO, modifierType, value);
            
            var effect = system.EffectSystem.GetEffect(effectSO, system, new MockParameters());
            var appliedEffect = system.EffectSystem.ApplyEffectToSelf(effect);
            return appliedEffect;
        }
    }

    public class MockParameters : AbilityParameters {}
    
    public class FalseCustomRequirement : EffectCustomApplicationRequirement 
    {
        public override bool CanApplyEffect(EffectScriptableObject effect, AbstractEffect effectSpec, AbilitySystemBehaviour ownerSystem)
        {
            return false;
        }
    }
    
    public class TrueCustomRequirement : EffectCustomApplicationRequirement 
    {
        public override bool CanApplyEffect(EffectScriptableObject effect, AbstractEffect effectSpec, AbilitySystemBehaviour ownerSystem)
        {
            return true;
        }
    }

}