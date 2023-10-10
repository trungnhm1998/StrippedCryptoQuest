using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.EffectCalculations
{
    public class DamageOverTimeCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _targetMaxHp;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            var effectSpec = (EffectSpec)executionParams.EffectSpec;
            if (effectSpec == null) return;
            SkillParameters skillParameters = effectSpec.Parameters;
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.targetAttribute;
            executionParams.TryGetAttributeValue(_targetMaxHp, out var targetMaxHp);
            var damageValue = targetMaxHp.CurrentValue / 20;
            var modifier = new EffectAttributeModifier()
            {
                Attribute = targetAttribute.Attribute,
                ModifierType = EAttributeModifierType.Add,
                Value = -damageValue
            };
            outModifiers.Add(modifier);
        }
    }
}