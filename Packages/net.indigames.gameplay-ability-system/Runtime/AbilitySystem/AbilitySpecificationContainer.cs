using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem
{
    public class AbilitySpecificationContainer
    {
        public List<GameplayAbilitySpec> Abilities = new List<GameplayAbilitySpec>();
        public AbilitySystemBehaviour Owner;
        
        public void RegisterWithOwner(AbilitySystemBehaviour owner)
        {
            Owner = owner;
        }
    }
}
