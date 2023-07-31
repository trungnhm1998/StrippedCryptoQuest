using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleOrder
{
    [RequireComponent(typeof(BattleManager))]
    public class PVEBattleOrderDecider : BattleOrderDecider
    {
        [SerializeField] private AttributeScriptableObject _enemyDecideAttribute;

        protected override float GetDecideAttributeValue(IBattleUnit unit)
        {
            var attributeSystem = unit.Owner.AttributeSystem;
            AttributeValue attributeValue;
            if (attributeSystem.GetAttributeValue(_playerDecideAttribute, out attributeValue))
            {
                return attributeValue.CurrentValue;
            }
            attributeSystem.GetAttributeValue(_enemyDecideAttribute, out attributeValue);
            return attributeValue.CurrentValue;
        }
    }
}
    