
using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterBuffManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> _regenerateStep;
        [SerializeField] private AttributeScriptableObject _encounterRateAttribute;
        [SerializeField] private AttributeChangeEvent _attributeChangedEvent;

        private void Start()
        {
            CalculateBuffFromParty();
        }

        private void OnEnable()
        {
            _attributeChangedEvent.Changed += AttributeChangedEvent;
        }

        private void OnDisable()
        {
            _attributeChangedEvent.Changed -= AttributeChangedEvent;
        }

        private void AttributeChangedEvent(AttributeSystemBehaviour owner, AttributeValue oldValue,
            AttributeValue newValue)
        {
            var changedAttribute = oldValue.Attribute;
            if (changedAttribute != _encounterRateAttribute) return;
            CalculateBuffFromParty();
        }

        private void CalculateBuffFromParty()
        {
            var party = ServiceProvider.GetService<IPartyController>();
            float passiveBuff = 0f;
            float externalBuff = 0f;

            foreach (var member in party.OrderedAliveMembers)
            {
                if (!member.AttributeSystem.TryGetAttributeValue(_encounterRateAttribute, out var buffValue))
                    continue;
                passiveBuff += buffValue.BaseValue;
                externalBuff += buffValue.CurrentValue - buffValue.BaseValue;
                
            }

            var finalBuff = 1 / ((1 - Mathf.Clamp01(-passiveBuff)) * (1 - Mathf.Clamp01(-externalBuff)));
            Debug.Log($"EncounterBuff:: Final buff value {finalBuff}");
            OnEncounterBuffApplied(finalBuff);
        }

        private void OnEncounterBuffApplied(float value)
        {
            _regenerateStep.Invoke(value);
        } 
    }
}