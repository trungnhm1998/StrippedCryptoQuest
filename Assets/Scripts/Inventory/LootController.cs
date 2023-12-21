using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class LootController : MonoBehaviour, ILootVisitor
    {
        [SerializeField] private PartyManager _partyManager;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _diamond;
        [SerializeField] private CurrencySO _soul;

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
            var amount = (int)loot.Item.Amount;
            if (loot.Item.Data == _gold)
                ActionDispatcher.Dispatch(new AddGoldAction(amount));

            if (loot.Item.Data == _diamond)
                ActionDispatcher.Dispatch(new AddDiamonds(amount));

            if (loot.Item.Data == _soul)
                ActionDispatcher.Dispatch(new AddSouls(amount));
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