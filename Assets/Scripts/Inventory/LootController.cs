using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class LootController : MonoBehaviour, ILootVisitor
    {
        [SerializeField] private PartyManager _partyManager;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        protected void OnEnable() => _addLootRequestEventChannel.EventRaised += AddLoot;

        protected void OnDisable() => _addLootRequestEventChannel.EventRaised -= AddLoot;

        private void AddLoot(LootInfo loot) => loot.Accept(this);

        public void Visit(ConsumableLootInfo loot)
        {
            ActionDispatcher.Dispatch(new AddConsumableAction(loot.Item.Data, loot.Item.Quantity));
        }

        public void Visit(CurrencyLootInfo loot)
        {
            // TODO: Implement
        }

        public void Visit(EquipmentLoot loot)
        {
            Debug.LogWarning($"Try to loot equipment {loot.EquipmentId} but haven't implemented yet");
            // TODO: Implement
        }

        public void Visit(ExpLoot loot)
        {
            foreach (var slot in _partyManager.Slots)
            {
                if (!slot.HeroBehaviour.IsValidAndAlive()) continue;
                var levelSystem = slot.HeroBehaviour.GetComponent<LevelSystem>();
                levelSystem.AddExp(loot.Exp);
            }
        }
    }
}