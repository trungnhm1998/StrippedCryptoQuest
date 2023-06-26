using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    public class SkillSpecificationContainer
    {
        public List<AbstractSkill> Skills = new List<AbstractSkill>();
        public AbilitySystem Owner;
        
        public void RegisterWithOwner(AbilitySystem owner)
        {
            Owner = owner;
        }
    }
}
