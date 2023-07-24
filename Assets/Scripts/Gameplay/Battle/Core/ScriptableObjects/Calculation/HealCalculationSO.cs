using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    [CreateAssetMenu(fileName = "HealCalculation",
        menuName = "Gameplay/Battle/Effects/Execution Calculations/Heal Calculation")]
    public class HealCalculationSO : AbstractEffectExecutionCalculationSO
    {
        [SerializeField] private AttributeScriptableObject _baseMagicAttackSO;
        [SerializeField] private AttributeScriptableObject _targetedAttributeSO;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] attributeModifiers)
        {
            SkillParameters skillParameters = effectSpec.Parameters as SkillParameters;
            effectSpec.Owner.AttributeSystem.GetAttributeValue(_baseMagicAttackSO, out AttributeValue baseMagicAttack);
            float healAmount = BattleCalculator.CalculateBaseDamage(skillParameters, baseMagicAttack.CurrentValue,
                Random.Range(-0.05f, 0.05f));
            for (var index = 0; index < attributeModifiers.Length; index++)
            {
                if (attributeModifiers[index].AttributeSO != _targetedAttributeSO) continue;

                EffectAttributeModifier previousModifier = attributeModifiers[index];
                attributeModifiers[index].Value = healAmount;

                return true;
            }

            return false;
        }
    }
}