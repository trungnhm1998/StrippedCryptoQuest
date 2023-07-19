using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.SkillSelectStrategies
{
    public interface ISkillSelector
    {
        AbstractAbility GetSkill(BattleUnitBase battleUnit);
    }
}