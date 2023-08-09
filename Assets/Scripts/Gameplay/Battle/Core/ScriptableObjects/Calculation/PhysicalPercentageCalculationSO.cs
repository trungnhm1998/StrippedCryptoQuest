using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public class PhysicalPercentageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        [SerializeField] private AttributeScriptableObject _ownerAttack;
        [SerializeField] private AttributeScriptableObject _targetDefense;
        [SerializeField] private AttributeScriptableObject _targetedAttribute;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] modifiers)
        {
            SkillParameters skillParameters = effectSpec.Parameters as SkillParameters;
            if (skillParameters == null) return false;
            
            effectSpec.Owner.AttributeSystem.GetAttributeValue(_ownerAttack, out AttributeValue attackValue);
            effectSpec.Target.AttributeSystem.GetAttributeValue(_targetDefense, out AttributeValue defenseValue);
            float baseDamageValue = BattleCalculator.CalculateBaseDamage(skillParameters, attackValue.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));
            float damageValue = 0;
            if (skillParameters.IsFixed)
                damageValue = BattleCalculator.CalculateFixedPhysicalDamage(baseDamageValue, 1, 1);
            else
                damageValue =
                    BattleCalculator.CalculatePercentPhysicalDamage(baseDamageValue, attackValue.CurrentValue,
                        defenseValue.CurrentValue, 1, 1);

            for (var index = 0; index < modifiers.Length; index++)
            {
                if (modifiers[index].AttributeSO != _targetedAttribute) continue;
                modifiers[index].Value = damageValue;

                return true;
            }

            return false;
        }
    }
}