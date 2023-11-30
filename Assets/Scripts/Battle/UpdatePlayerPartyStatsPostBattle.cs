using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class UpdatePlayerPartyStatsPostBattle : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _unloadingEvent;

        private void Awake()
        {
            _unloadingEvent = BattleEventBus.SubscribeEvent<UnloadingEvent>(UpdatePartyStats);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_unloadingEvent);
        }

        private void UpdatePartyStats(UnloadingEvent ctx)
        {
            var partyController = ServiceProvider.GetService<IPartyController>();
            foreach (var slot in partyController.Slots)
            {
                if (slot.IsValid() == false) continue;
                // this will also remove expired effects and update the attribute values
                slot.HeroBehaviour.GameplayEffectSystem.UpdateAttributeModifiersUsingAppliedEffects();
            }
        }
    }
}