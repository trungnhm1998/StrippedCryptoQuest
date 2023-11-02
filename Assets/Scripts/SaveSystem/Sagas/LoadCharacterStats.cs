using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Sagas;
using CryptoQuest.SaveSystem.Actions;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.SaveSystem.Sagas
{
    public class LoadCharacterStats : SagaBase<PartyInitialized>
    {
        protected override void HandleAction(PartyInitialized ctx) => StartCoroutine(LoadStatsCo());

        private IEnumerator LoadStatsCo()
        {
            var save = PlayerPrefs.GetString("characters");
            if (string.IsNullOrEmpty(save)) yield break;
            var characters = JsonConvert
                .DeserializeObject<List<CharacterStats>>(save).ToDictionary(obj => obj.Id);
            var party = ServiceProvider.GetService<IPartyController>();
            foreach (var partySlot in party.Slots)
            {
                if (!partySlot.IsValid()) continue;
                var attributeSystem = partySlot.HeroBehaviour.AttributeSystem;
                var character = characters[partySlot.HeroBehaviour.Spec.Id];
                foreach (var attribute in character.Attributes)
                {
                    var handle = Addressables.LoadAssetAsync<AttributeScriptableObject>(attribute.AttributeGuid);
                    yield return handle; // this should instantly in this frame
                    var attributeSO = handle.Result;
                    
                    attributeSystem.SetAttributeBaseValue(attributeSO, attribute.Value);
                }
            }
        }
    }
}