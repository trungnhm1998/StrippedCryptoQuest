using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    public class AbilitySpecificationContainer
    {
        public List<AbstractAbility> Abilities = new List<AbstractAbility>();
        public AbilitySystemBehaviour Owner;
        
        public void RegisterWithOwner(AbilitySystemBehaviour owner)
        {
            Owner = owner;
        }
    }
}
