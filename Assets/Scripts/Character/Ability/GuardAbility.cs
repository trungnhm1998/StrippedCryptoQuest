using System.Collections;
using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// <see cref="AbilityTags"/> should contains the guard tag
    /// </summary>
    public class GuardAbility : AbilityScriptableObject<GuardAbilitySpec> { }

    /// <summary>
    /// this doesn't do anything for now, beside when the ability is active, the character will have a guard tag
    ///
    /// post damage calculation will be done with tag in mind in <see cref="ReduceIncomingDamageWhenGuarded"/>
    /// </summary>
    public class GuardAbilitySpec : GameplayAbilitySpec
    {
        protected override IEnumerator OnAbilityActive()
        {
            yield break;
        }
    }
}