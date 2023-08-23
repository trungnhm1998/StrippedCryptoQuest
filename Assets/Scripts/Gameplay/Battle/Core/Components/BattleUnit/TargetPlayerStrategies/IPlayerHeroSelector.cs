using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.TargetPlayerStrategies
{
    public interface IPlayerHeroSelector
    {
        AbilitySystemBehaviour GetTarget(MonsterUnit monsterUnit);
    }
}