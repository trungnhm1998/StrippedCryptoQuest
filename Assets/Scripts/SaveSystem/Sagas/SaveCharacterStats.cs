using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.SaveSystem.Sagas
{
    [Serializable]
    internal struct CharacterStats
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("stats")]
        public List<SaveAttributeValue> Attributes;
    }

    public class SaveCharacterStats : MonoBehaviour
    {
        [SerializeField]
        private AttributeScriptableObject[] _attributeToSave = Array.Empty<AttributeScriptableObject>();

        [SerializeField] private SceneScriptableObject _battleScene;

        private void OnEnable()
        {
            AdditiveGameSceneLoader.SceneUnloaded += SaveCharacterStatsAfterBattle;
        }

        private void OnDisable()
        {
            AdditiveGameSceneLoader.SceneUnloaded -= SaveCharacterStatsAfterBattle;
        }

        private void SaveCharacterStatsAfterBattle(SceneScriptableObject unloadedScene)
        {
            if (unloadedScene != _battleScene) return;
            var party = ServiceProvider.GetService<IPartyController>();
            var characters = new List<CharacterStats>();

            foreach (var partySlot in party.Slots)
            {
                if (!partySlot.IsValid()) continue;
                var hero = partySlot.HeroBehaviour;
                var attributeSystem = hero.AttributeSystem;
                characters.Add(new CharacterStats
                {
                    Id = hero.Spec.Id,
                    Attributes = GetRuntimeStats(attributeSystem)
                });
            }
            
            PlayerPrefs.SetString("characters", JsonConvert.SerializeObject(characters));
            
        }

        private List<SaveAttributeValue> GetRuntimeStats(AttributeSystemBehaviour attributeSystem)
        {
            var attributes = new List<SaveAttributeValue>();

            foreach (var attribute in _attributeToSave)
            {
                attributeSystem.TryGetAttributeValue(attribute, out var attributeValue);
                attributes.Add(new SaveAttributeValue
                {
                    AttributeGuid = attribute.Guid, // use this to load the attribute
                    Value = attributeValue.CurrentValue // using current value because it's the runtime value
                });
            }

            return attributes;
        }
    }
}