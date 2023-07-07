using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle
{
    public interface ISkillSelector
    {
        AbstractAbility GetSkill(BattleUnitBase battleUnit);
    }
}