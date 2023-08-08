using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class NormalAttackHandler : BattleActionHandler
    {
        public override void Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return;
            currentUnit.SelectAbility(currentUnit.UnitLogic.NormalAttack);
            NextHandler?.Handle(currentUnit);
        }
    }
}