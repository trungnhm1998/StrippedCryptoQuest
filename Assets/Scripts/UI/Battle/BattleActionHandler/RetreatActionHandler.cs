using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class RetreatActionHandler : BattleActionHandler
    {
        public AbilityScriptableObject RetreatAbilitySO;
        public AttributeScriptableObject TargetedAttributeSO;
        public VoidEventChannelSO _onRetreatFailedEvent;

        public override void Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return;
            if (!IsAbleToRetreat(currentUnit))
            {
                return;
            }

            Retreat(currentUnit);
        }

        private void Retreat(IBattleUnit currentUnit)
        {
            AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;
            AbstractAbility retreatAbility = currentUnitOwner.GiveAbility(RetreatAbilitySO);
            currentUnit.SelectSkill(retreatAbility);
            currentUnit.SelectSingleTarget(currentUnitOwner);
        }

        private float GetTargetMaxAttributeValue(IBattleUnit currentUnit)
        {
            float targetMaxAttributeValue = 0;
            foreach (var target in currentUnit.OpponentTeam.Members)
            {
                target.AttributeSystem.GetAttributeValue(TargetedAttributeSO, out var targetAttributeValue);
                targetMaxAttributeValue = Mathf.Max(targetMaxAttributeValue, targetAttributeValue.CurrentValue);
            }

            return targetMaxAttributeValue;
        }

        private float GetOwnerAttributeValue(IBattleUnit currentUnit)
        {
            currentUnit.Owner.AttributeSystem.GetAttributeValue(TargetedAttributeSO, out var ownerAttributeValue);
            return ownerAttributeValue.CurrentValue;
        }

        private bool IsAbleToRetreat(IBattleUnit currentUnit)
        {
            float probabilityOfRetreat =
                BattleCalculator.CalculateProbabilityOfRetreat(GetTargetMaxAttributeValue(currentUnit),
                    GetOwnerAttributeValue(currentUnit));
            float randomValue = Random.value;
            Debug.Log("Probability of Retreat: " + probabilityOfRetreat + " Random value: " + randomValue);
            Debug.Log("Is Battle Escapable: " + CurrentBattleInfo.IsBattleEscapable + " Is Able To Retreat: " +
                      (randomValue < probabilityOfRetreat && CurrentBattleInfo.IsBattleEscapable));
            return randomValue < probabilityOfRetreat && CurrentBattleInfo.IsBattleEscapable;
        }
    }
}