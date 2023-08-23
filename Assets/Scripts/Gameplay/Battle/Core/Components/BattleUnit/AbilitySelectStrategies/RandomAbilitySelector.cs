using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies
{
    public class RandomAbilitySelector : IAbilitySelector
    {
        public GameplayAbilitySpec GetAbility(IBattleUnit battleUnit)
        {
            var grantedAbilities = battleUnit.Owner.GrantedAbilities;
            if (grantedAbilities.Count <= 0) return null;
            var ramdomAbility = grantedAbilities[Random.Range(0, grantedAbilities.Count - 1)];
            return ramdomAbility;
        }
    }
}