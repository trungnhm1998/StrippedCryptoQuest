using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    public class SkillSpecificationContainer
    {
        public List<AbstractSkill> Skills = new List<AbstractSkill>();
        public SkillSystem Owner;
        
        public void RegisterWithOwner(SkillSystem owner)
        {
            Owner = owner;
        }
    }
}
