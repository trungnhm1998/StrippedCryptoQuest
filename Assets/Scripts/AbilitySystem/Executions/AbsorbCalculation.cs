using CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class AbsorbCalculation : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var specContext = spec.Context.Get();
            if (specContext is not PostDamageContext context) return;

            executionParams.SourceAbilitySystemComponent.AttributeSystem.TryGetAttributeValue(AttributeSets.MagicAttack,
                out var magicPower);
            var skillParameters = context.SkillInfo.SkillParameters;
            var absorbDamage = skillParameters.IsFixed ? -context.DamageContext.Damage
                : -context.DamageContext.Damage * skillParameters.BasePower/100f;
            Debug.Log($"Absorb [{absorbDamage}] damage dealt was [{context.DamageContext.Damage}]");
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = skillParameters.TargetAttribute.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = absorbDamage
            };
            outModifiers.Add(modifier);
            BattleEventBus.RaiseEvent(new AbsorbingEvent()
            {
                Character = context.DamageContext.Target,
                AbsorbingAttribute = modifier.Attribute,
                Value = modifier.Magnitude,
            });
        }
    }
}