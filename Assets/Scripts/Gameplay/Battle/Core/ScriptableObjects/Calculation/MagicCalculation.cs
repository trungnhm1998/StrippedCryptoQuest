using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    [CreateAssetMenu(fileName = "HealCalculation",
        menuName = "Gameplay/Battle/Effects/Execution Calculations/Heal Calculation")]
    public class MagicCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private AttributeScriptableObject _targetedAttributeSO;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            CryptoQuestGameplayEffectSpec effectSpec = (CryptoQuestGameplayEffectSpec)executionParams.EffectSpec;
            SkillParameters skillParameters = effectSpec.SkillParam;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseMagicAttack);

            float baseMagicValue = BattleCalculator.CalculateBaseDamage(skillParameters, baseMagicAttack.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));

            if (baseMagicValue > 0f)
            {
                var modifier = new EffectAttributeModifier()
                {
                    Attribute = _targetedAttributeSO,
                    ModifierType = EAttributeModifierType.Add,
                    Value = baseMagicValue
                };
                outModifiers.Add(modifier);
            }
        }
    }
}