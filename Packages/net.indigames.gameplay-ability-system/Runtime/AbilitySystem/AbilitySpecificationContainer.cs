using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem
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
