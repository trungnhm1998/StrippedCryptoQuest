using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// Base class for consumable such as herb, potion, etc.
    ///
    /// Ocarina should derived from this for logic
    /// </summary>
    public class ConsumableAbility : AbilityScriptableObject<ConsumableAbilitySpec> { }

    public class ConsumableAbilitySpec : GameplayAbilitySpec { }
}