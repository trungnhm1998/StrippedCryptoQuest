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
        public VoidEventChannelSO _onEscapeFailedEvent;

        public override void Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return;
            float probabilityOfEscape =
                BattleCalculator.CalculateProbabilityOfEscape(GetTargetMaxAttributeValue(currentUnit),
                    GetOwnerAttributeValue(currentUnit));
            float randomValue = Random.value;
            Debug.Log("Probability of escape: " + probabilityOfEscape + " Random value: " + randomValue);
            if (randomValue > probabilityOfEscape || !CurrentBattleInfo.IsBattleEscapable)
            {
                _onEscapeFailedEvent.RaiseEvent();
                return;
            }

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
                if (targetAttributeValue.CurrentValue > targetMaxAttributeValue)
                {
                    targetMaxAttributeValue = targetAttributeValue.CurrentValue;
                }
            }

            return targetMaxAttributeValue;
        }

        private float GetOwnerAttributeValue(IBattleUnit currentUnit)
        {
            currentUnit.Owner.AttributeSystem.GetAttributeValue(TargetedAttributeSO, out var ownerAttributeValue);
            return ownerAttributeValue.CurrentValue;
        }
    }
}