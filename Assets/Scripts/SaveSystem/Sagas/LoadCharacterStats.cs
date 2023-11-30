using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Sagas;
using CryptoQuest.SaveSystem.Actions;
using CryptoQuest.SaveSystem.Sagas.ScriptableObjects;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.SaveSystem.Sagas
{
    public class LoadCharacterStats : SagaBase<PartyInitialized>
    {
        [SerializeField] private StatsSaveConfig _config;

        protected override void HandleAction(PartyInitialized ctx)
        {
            var save = PlayerPrefs.GetString("characters");
            if (string.IsNullOrEmpty(save)) return;
            var characters = JsonConvert
                .DeserializeObject<List<CharacterStats>>(save).ToDictionary(obj => obj.Id);
            var party = ServiceProvider.GetService<IPartyController>();
            foreach (var partySlot in party.Slots)
            {
                if (!partySlot.IsValid()) continue;
                var attributeSystem = partySlot.HeroBehaviour.AttributeSystem;
                var character = characters[partySlot.HeroBehaviour.Spec.Id];
                OverrideCharacterStats(character, attributeSystem);
            }
        }

        private void OverrideCharacterStats(CharacterStats character, AttributeSystemBehaviour attributeSystem)
        {
            foreach (var saveAttribute in character.Attributes)
            {
                var attribute = _config.AttributeByName[saveAttribute.Name];
                attributeSystem.SetAttributeBaseValue(attribute, saveAttribute.Value);
            }
        }
    }
}