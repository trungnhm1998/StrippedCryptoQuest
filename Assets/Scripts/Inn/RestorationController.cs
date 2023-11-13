using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Inn
{
    public class RestorationController : MonoBehaviour
    {
        [Header("Ability System")]
        [SerializeField] private GameplayEffectDefinition _healToFullEffectDefinition;

        public bool RestoreParty()
        {
            IPartyController party = ServiceProvider.GetService<IPartyController>();

            foreach (PartySlot partySlot in party.Slots)
            {
                if (!partySlot.IsValid() || !partySlot.HeroBehaviour.IsValidAndAlive()) continue;

                HeroBehaviour hero = partySlot.HeroBehaviour;
                AbilitySystemBehaviour abilitySystem = hero.GetComponent<AbilitySystemBehaviour>();

                GameplayEffectSpec effect = abilitySystem.MakeOutgoingSpec(_healToFullEffectDefinition);

                abilitySystem.ApplyEffectSpecToSelf(effect);
            }

            return true;
        }
    }
}