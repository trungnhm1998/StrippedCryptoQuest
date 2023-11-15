using CryptoQuest.Church.Interface;
using CryptoQuest.Church.UI;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Church
{
    public class CharacterReviver : MonoBehaviour, IPartyReviver
    {
        [SerializeField] private GameplayEffectDefinition _reviveEffectDefinition;
        public void ReviveCharacter(UICharacter character)
        {
            AbilitySystemBehaviour abilitySystem = character.HeroBehaviour.GetComponent<AbilitySystemBehaviour>();
            abilitySystem.ApplyEffectSpecToSelf(abilitySystem.MakeOutgoingSpec(_reviveEffectDefinition));
        }
    }
}
