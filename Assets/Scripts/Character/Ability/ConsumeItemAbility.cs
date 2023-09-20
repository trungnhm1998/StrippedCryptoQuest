using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// Base data class for consumable such as herb, potion, etc.
    /// </summary>
    public abstract class ConsumeItemAbility : AbilityScriptableObject
    {
        public event Action ItemConsumed;

        public void OnItemConsumed()
        {
            ItemConsumed?.Invoke();
        }

        /// <summary>
        /// Derived class should raise event to show correct UI if there is any
        ///
        /// currently we have behavior:
        /// - Single target hero
        /// - Ocarina with special UI flow
        /// - Target all hero in party (This doesn't have UI flow yet)
        /// </summary>
        public abstract void Consuming();
    }

    /// <summary>
    /// Base logic class for consumable ability
    ///
    /// Ocarina should derived from this for logic
    /// </summary>
    public abstract class ConsumableAbilitySpec : GameplayAbilitySpec { }
}