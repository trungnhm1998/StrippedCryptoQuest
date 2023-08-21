using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleOrder
{
    [RequireComponent(typeof(BattleManager))]
    public class BattleOrderDecider : MonoBehaviour
    {
        [SerializeField] protected AttributeScriptableObject _decideAttribute;

        public List<IBattleUnit> SortUnitByAttributeValue(List<IBattleUnit> units, bool isDescending = true)
        {
            var orderByDescendingList = units.OrderByDescending(unit => GetDecideAttributeValue(unit)).ToList();
            if (!isDescending)
            {
                orderByDescendingList.Reverse();
            }
            return orderByDescendingList;
        }

        protected virtual float GetDecideAttributeValue(IBattleUnit unit)
        {
            var attributeSystem = unit.Owner.AttributeSystem;
            attributeSystem.TryGetAttributeValue(_decideAttribute, out var attributeValue);
            return attributeValue.CurrentValue;
        }
    }
}
    