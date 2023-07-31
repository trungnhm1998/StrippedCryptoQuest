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
        [SerializeField] private AbilityScriptableObject _retreatAbilitySO;
        [SerializeField] private AttributeScriptableObject _targetedAttributeSO;
        [SerializeField] private BattlePanelController _battlePanelController;

        public override void Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return;
            if (!IsAbleToRetreat(currentUnit))
            {
                //TODO: Move retreat logic to ability and add action in retreat ability if can retreat or not
                _battlePanelController.ReinitializeUI();
                return;
            }

            Retreat(currentUnit);
        }

        private void Retreat(IBattleUnit currentUnit)
        {
            AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;
            AbstractAbility retreatAbility = currentUnitOwner.GiveAbility(_retreatAbilitySO);
            currentUnit.SelectSkill(retreatAbility);
            currentUnit.SelectSingleTarget(currentUnitOwner);
        }

        private float GetTargetMaxAttributeValue(IBattleUnit currentUnit)
        {
            float targetMaxAttributeValue = 0;
            foreach (var target in currentUnit.OpponentTeam.Members)
            {
                target.AttributeSystem.GetAttributeValue(_targetedAttributeSO, out var targetAttributeValue);
                targetMaxAttributeValue = Mathf.Max(targetMaxAttributeValue, targetAttributeValue.CurrentValue);
            }

            return targetMaxAttributeValue;
        }

        private float GetOwnerAttributeValue(IBattleUnit currentUnit)
        {
            currentUnit.Owner.AttributeSystem.GetAttributeValue(_targetedAttributeSO, out var ownerAttributeValue);
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