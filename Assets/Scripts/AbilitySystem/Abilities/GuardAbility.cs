using System;
using System.Collections;
using CryptoQuest.AbilitySystem.Executions;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// <see cref="AbilityTags"/> should contains the guard tag
    /// </summary>
    public class GuardAbility : AbilityScriptableObject<GuardAbilitySpec>
    {
        [field: SerializeField] public LocalizedString Description { get; private set; }
    }

    /// <summary>
    /// this doesn't do anything for now, beside when the ability is active, the character will have a guard tag
    ///
    /// post damage calculation will be done with tag in mind in <see cref="ReduceIncomingDamageWhenGuarded"/>
    /// </summary>
    public class GuardAbilitySpec : GameplayAbilitySpec
    {
        public event Action GuardActivatedEvent;
        protected override IEnumerator OnAbilityActive()
        {
            GuardActivatedEvent?.Invoke();
            EndAbility();
            yield break;
        }
    }
}