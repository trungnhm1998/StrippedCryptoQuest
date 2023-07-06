using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle
{
    public interface ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit);
    }
}