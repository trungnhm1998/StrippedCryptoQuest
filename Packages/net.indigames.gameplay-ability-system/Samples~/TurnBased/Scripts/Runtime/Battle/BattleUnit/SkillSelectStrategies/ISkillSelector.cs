using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    public interface ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit);
    }
}