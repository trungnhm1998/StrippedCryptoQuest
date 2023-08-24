using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay
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
            var character = executionParams.SourceAbilitySystemComponent.GetComponent<CharacterBehaviour>();
            var characterElemental = character.Element;

            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = characterElemental.AttackAttribute,
                CaptureFrom = EGameplayEffectCaptureSource.Source
            }, out var elementalAtk, 1f);

            executionParams.TryGetAttributeValue(new CustomExecutionAttributeCaptureDef()
            {
                Attribute = characterElemental.ResistanceAttribute,
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

            if (damageDone > 0f)
            {
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
}