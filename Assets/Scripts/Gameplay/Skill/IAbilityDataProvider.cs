using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Skill
{
    public interface IAbilityDataProvider
    {
        public List<SkillInformation> GetAllAbility();
    }
}