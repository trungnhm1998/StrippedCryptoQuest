using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "Escape Ability", menuName = "Gameplay/Battle/Abilities/Escape Ability")]
    public class RetreatAbilitySO : SpecialAbilitySO
    {
        [SerializeField] private TargetType _targetType;
        [SerializeField] private AttributeScriptableObject _targetedAttribute;
        public UnityAction OnRetreatSucceed;
        public UnityAction OnRetreatFailed;

        protected override AbstractAbility CreateAbility()
            => new RetreatAbility(_targetType, _targetedAttribute);
    }

    public class RetreatAbility : SpecialAbility
    {
        protected new RetreatAbilitySO AbilitySO => (RetreatAbilitySO)_abilitySO;
        private TargetType _targetType;
        private AttributeScriptableObject _targetedAttribute;

        public RetreatAbility(TargetType targetType, AttributeScriptableObject targetedAttribute)
        {
            _targetType = targetType;
            _targetedAttribute = targetedAttribute;
        }

        public override IEnumerator AbilityActivated()
        {
            HandleRetreat();
            yield return base.AbilityActivated();
        }

        private void HandleRetreat()
        {
            float probabilityOfEscape =
                BattleCalculator.CalculateProbabilityOfEscape(GetTargetMaxAttributeValue(),
                    GetOwnerAttributeValue());
            float randomValue = Random.value;
            if (randomValue <= probabilityOfEscape)
                AbilitySO.OnRetreatSucceed?.Invoke();
            else
                AbilitySO.OnRetreatFailed?.Invoke();
        }


        private float GetTargetMaxAttributeValue()
        {
            List<AbilitySystemBehaviour> targets = new();
            _targetType.GetTargets(Owner, ref targets);
            float targetMaxAttributeValue = 0;
            foreach (var target in targets)
            {
                target.AttributeSystem.GetAttributeValue(_targetedAttribute, out var targetAttributeValue);
                if (targetAttributeValue.CurrentValue > targetMaxAttributeValue)
                {
                    targetMaxAttributeValue = targetAttributeValue.CurrentValue;
                }
            }

            return targetMaxAttributeValue;
        }

        private float GetOwnerAttributeValue()
        {
            Owner.AttributeSystem.GetAttributeValue(_targetedAttribute, out var ownerAttributeValue);
            return ownerAttributeValue.CurrentValue;
        }
    }
}