using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class LootController : MonoBehaviour, ILootVisitor
    {
        [SerializeField] private PartyManager _partyManager;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IInventoryController _inventory;

        private void Awake() => _inventory ??= ServiceProvider.GetService<IInventoryController>();

        protected void OnEnable() => _addLootRequestEventChannel.EventRaised += AddLoot;

        protected void OnDisable() => _addLootRequestEventChannel.EventRaised -= AddLoot;

        private void AddLoot(LootInfo loot) => loot.Accept(this);

        public void Visit(ConsumableLootInfo loot)
        {
            var consumable = new ConsumableInfo(loot.Item.Data, loot.Item.Quantity);
            _inventory.Add(consumable);
        }

        public void Visit(CurrencyLootInfo loot)
        {
            var currency = new CurrencyInfo(loot.Item.Data, loot.Item.Amount);
            _inventory.Add(currency);
        }

        public void Visit(EquipmentLoot loot)
        {
            Debug.LogWarning($"Try to loot equipment {loot.EquipmentId} but haven't implemented yet");
            // TODO: Implement this
            // var equipment = new Equipment(loot.EquipmentSO);
            // _inventory.Add(equipment);
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