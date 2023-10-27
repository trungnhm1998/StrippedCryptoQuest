using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using CoreEffectContext = IndiGames.GameplayAbilitySystem.EffectSystem.GameplayEffectContext;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class AbsorbCalculation : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var specContext = spec.Context.Get();
            if (specContext is not PostNormalAttackContext context) return;

            executionParams.SourceAbilitySystemComponent.AttributeSystem.TryGetAttributeValue(AttributeSets.MagicAttack,
                out var magicPower);
            var skillParameters = context.SkillInfo.SkillParameters;
            var skillPower =
                BattleCalculator.CalculateMagicSkillBasePower(skillParameters, magicPower.CurrentValue);
            var absorbDamage = context.AttackContext.Damage / skillPower;
            Debug.Log($"Absorb [{absorbDamage}] damage dealt was [{context.AttackContext.Damage}] skill power [{skillPower}]");
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = skillParameters.targetAttribute.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = absorbDamage
            };
            outModifiers.Add(modifier);
            BattleEventBus.RaiseEvent(new AbsorbingEvent()
            {
                Character = context.AttackContext.Target,
                AbsorbingAttribute = modifier.Attribute,
                Value = modifier.Magnitude,
            });
        }
    }
}