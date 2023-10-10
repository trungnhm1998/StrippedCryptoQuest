using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.EffectCalculations
{
    public class DamageCalculation : EffectExecutionCalculationBase
    {
        [Header("Config")]
        [SerializeField] private float _lowerRandomRange = -0.05f;

        [SerializeField] private float _upperRandomRange = 0.05f;
        [SerializeField] private AttributeScriptableObject _hp;
        [SerializeField] private CustomExecutionAttributeCaptureDef _captureAttack;
        [SerializeField] private CustomExecutionAttributeCaptureDef _captureDefense;

        public override void Execute(
            ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            var element = executionParams.SourceAbilitySystemComponent.GetComponent<Element>().ElementValue;

            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = element.AttackAttribute,
                CaptureFrom = EGameplayEffectCaptureSource.Source
            }, out var elementalAtk, 1f);

            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = element.ResistanceAttribute,
                CaptureFrom = EGameplayEffectCaptureSource.Target
            }, out var elementalDef, 1f);

            executionParams.TryGetAttributeValue(_captureDefense, out var enemyDefense, 1f);
            executionParams.TryGetAttributeValue(_captureAttack, out var attack, 1f);
            var correctedElementalPower = elementalAtk.CurrentValue / elementalDef.CurrentValue;
            if (correctedElementalPower == 0)
                correctedElementalPower = 1f;

            var damageDone =
                (attack.CurrentValue.CorrectAttack() - enemyDefense.CurrentValue.CorrectDefense()) *
                correctedElementalPower;
            damageDone += (damageDone * Random.Range(_lowerRandomRange, _upperRandomRange));

            if (damageDone <= 0f) return;

            Debug.Log($"Damage done: {damageDone}");
            var modifier = new EffectAttributeModifier()
            {
                Attribute = _hp,
                ModifierType = EAttributeModifierType.Add,
                Value = -damageDone
            };
            outModifiers.Add(modifier);
        }
    }
}