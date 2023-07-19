using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class NormalAttackHandler : BattleActionHandler
    {
        public override object Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return currentUnit;
            AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;
            if (currentUnit.NormalAttack == null)
            {
                Debug.LogWarning($"This character dont have normal attack skill");
                return currentUnit;
            }
            currentUnit.SelectSkill(currentUnit.NormalAttack);
            return base.Handle(currentUnit);
        }
    }
}