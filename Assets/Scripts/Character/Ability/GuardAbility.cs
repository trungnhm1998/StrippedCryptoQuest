using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    public class GuardAbility : AbilityScriptableObject<GuardAbilitySpec> { }

    public class GuardAbilitySpec : GameplayAbilitySpec { }
}