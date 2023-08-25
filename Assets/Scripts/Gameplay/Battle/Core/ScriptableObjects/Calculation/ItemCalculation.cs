using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Effects/Execution Calculations/Item Calculation",
        fileName = "Item Calculation")]
    public class ItemCalculation : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            CryptoQuestGameplayEffectSpec effectSpec = (CryptoQuestGameplayEffectSpec)executionParams.EffectSpec;
            SkillParameters skillParameters = effectSpec.SkillParam;

            CustomExecutionAttributeCaptureDef targetedAttribute = skillParameters.targetAttribute;

            AttributeScriptableObject attribute = targetedAttribute.Attribute;
            float value = skillParameters.BasePower;

            EAttributeModifierType modifierType = EAttributeModifierType.Add;

            if (!skillParameters.IsFixed)
            {
                modifierType = EAttributeModifierType.Multiply;
            }

            EffectAttributeModifier modifier = new EffectAttributeModifier()
            {
                Attribute = attribute,
                ModifierType = modifierType,
                Value = value,
            };
            outModifiers.Add(modifier);
        }
    }
}