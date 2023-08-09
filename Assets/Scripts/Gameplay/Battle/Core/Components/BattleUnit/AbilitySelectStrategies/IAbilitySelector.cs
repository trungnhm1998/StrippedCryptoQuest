using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies
{
    public interface IAbilitySelector
    {
        AbstractAbility GetAbility(IBattleUnit battleUnit);
    }
}