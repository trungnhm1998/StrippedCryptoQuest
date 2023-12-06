using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class PassivesController : CharacterComponentBase
    {
        public PassiveAbilitySpec ApplyPassive(PassiveAbility passive)
        {
            if (passive.Context.SkillInfo.SkillType == ESkillType.Passive) passive = Instantiate(passive);
            return Character.AbilitySystem.GiveAbility(passive) as PassiveAbilitySpec;
        }

        public void RemovePassive(PassiveAbilitySpec passive)
        {
            if (passive.SkillContext.SkillInfo.SkillType == ESkillType.Passive)
            {
                Destroy(passive.AbilitySO);
                passive.AbilitySO = null;
            }

            Character.AbilitySystem.RemoveAbility(passive);
        }
    }
}