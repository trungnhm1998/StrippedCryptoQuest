using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies
{
    public class RandomAbilitySelector : IAbilitySelector
    {
        public AbstractAbility GetAbility(BattleUnitBase battleUnit)
        {
            var grantedAbilities = battleUnit.Owner.GrantedAbilities.Abilities;
            if (grantedAbilities.Count <= 0) return null;
            var ramdomAbility = grantedAbilities[Random.Range(0, grantedAbilities.Count - 1)];
            return ramdomAbility;
        }
    }
}