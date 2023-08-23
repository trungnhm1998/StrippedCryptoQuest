using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies
{
    public interface IAbilitySelector
    {
        GameplayAbilitySpec GetAbility(IBattleUnit battleUnit);
    }
}