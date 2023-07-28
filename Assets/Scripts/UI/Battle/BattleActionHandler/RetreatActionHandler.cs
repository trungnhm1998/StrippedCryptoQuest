using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class RetreatActionHandler : BattleActionHandler
    {
        public AbilityScriptableObject RetreatAbilitySO;
        public bool IsBattleRetreatable;

        public override void Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return;
            AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;
            AbstractAbility retreatAbility = currentUnitOwner.GiveAbility(RetreatAbilitySO);
            // currentUnitOwner.TryActiveAbility(retreatAbility);
            currentUnit.SelectSkill(retreatAbility);
            currentUnit.SelectSingleTarget(currentUnitOwner);
        }
    }
}