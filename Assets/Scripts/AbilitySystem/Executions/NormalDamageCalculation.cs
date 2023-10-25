using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class NormalDamageCalculation : EffectExecutionCalculationBase
    {
        [Header("Config")]
        [SerializeField] private float _lowerRandomRange = -0.05f;

        [SerializeField] private float _upperRandomRange = 0.05f;
        [SerializeField] private AttributeScriptableObject _hp;
        [SerializeField] private CustomExecutionAttributeCaptureDef _captureAttack;
        [SerializeField] private CustomExecutionAttributeCaptureDef _captureDefense;

        public override void Execute(
            ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
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

            if (damageDone <= 0f) damageDone = 0;
            Debug.Log($"Damage done: {damageDone}");
            outModifiers.Add(new GameplayModifierEvaluatedData(
                // TODO: Would it be better if I add Damage attribute that will be -health?
                // Something like ActionRPG::URPGAttributeSet::PostGameplayEffectExecute
                // where only after the effect finishing apply the damage we will use that to modify the health attribute
                _hp,
                EAttributeModifierOperationType.Add,
                -Mathf.RoundToInt(damageDone))); // So I don't have to do this
        }
    }
}