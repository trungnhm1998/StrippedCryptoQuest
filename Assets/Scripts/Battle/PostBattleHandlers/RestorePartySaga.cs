using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using System;
using UniRx;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Battle.Components;
using CryptoQuest.AbilitySystem.Attributes;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class RestorePartyAction : ActionBase { } 

    public class RestorePartySaga : SagaBase<RestorePartyAction>
    {
        [SerializeField] private AttributeWithMaxCapped[] _resetAttributes;
        private IPartyController _party;

        protected override void HandleAction(RestorePartyAction ctx)
        {
            RestoreCharacter();
        }

        private void RestoreCharacter()
        {
            _party ??= ServiceProvider.GetService<IPartyController>();

            foreach (var character in _party.Slots)
            {
                if (character.IsValid())
                {
                    ResetAttributeValue(character.HeroBehaviour);
                }
            }
        }

        private void ResetAttributeValue(HeroBehaviour hero)
        {
            for (int i = 0; i < _resetAttributes.Length; i++)
            {
                var attributeToReset = _resetAttributes[i];
                if (!hero.AttributeSystem.TryGetAttributeValue(attributeToReset.CappedAttribute,
                        out var cappedAttributeValue)) continue;
                hero.AttributeSystem.SetAttributeBaseValue(attributeToReset, cappedAttributeValue.CurrentValue);
            }
        }
    }
}