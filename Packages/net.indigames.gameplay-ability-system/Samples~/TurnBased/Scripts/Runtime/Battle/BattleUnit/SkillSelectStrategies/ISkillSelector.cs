using Indigames.AbilitySystem;

namespace Indigames.AbilitySystem.Sample
{
    public interface ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit);
    }
}