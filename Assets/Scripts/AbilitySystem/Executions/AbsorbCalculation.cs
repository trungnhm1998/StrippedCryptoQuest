using CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
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
            var sourceSystem = executionParams.SourceAbilitySystemComponent.AttributeSystem;
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);

            var lastDamageDealt = sourceSystem.GetComponent<LastDamageDealtBehaviour>();
            var damageDealt = lastDamageDealt.LastDamageDealt;

            sourceSystem.TryGetAttributeValue(AttributeSets.MagicAttack,
                out var magicPower);
            var skillParameters = context.SkillInfo.SkillParameters;
            var absorbDamage = skillParameters.IsFixed ? -damageDealt
                : -damageDealt * skillParameters.BasePower/100f;
            Debug.Log($"Absorb [{absorbDamage}] damage dealt was [{damageDealt}]");
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = skillParameters.TargetAttribute.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = absorbDamage
            };
            outModifiers.Add(modifier);
            BattleEventBus.RaiseEvent(new AbsorbingEvent()
            {
                Character = lastDamageDealt.DamageReceiver,
                AbsorbingAttribute = modifier.Attribute,
                Value = modifier.Magnitude,
            });
        }
    }
}