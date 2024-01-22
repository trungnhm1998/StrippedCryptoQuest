using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.LootAPI;
using CryptoQuest.Item.MagicStone.Sagas;
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

        [Header("Listening to")] [SerializeField]
        private LootEventChannelSO _addLootRequestEventChannel;

        protected void OnEnable() => _addLootRequestEventChannel.EventRaised += AddLoot;

        protected void OnDisable() => _addLootRequestEventChannel.EventRaised -= AddLoot;

        private void AddLoot(LootInfo loot) => loot.Accept(this);

        public void Visit(ConsumableLootInfo loot)
        {
            ActionDispatcher.Dispatch(new AddConsumableToServerAction(loot.Item.Data, loot.Item.Quantity));
        }

        public void Visit(CurrencyLootInfo loot)
        {
            var amount = (int)loot.Item.Amount;
            if (loot.Item.Data == _gold)
                ActionDispatcher.Dispatch(new AddGoldToServerAction(amount));

            if (loot.Item.Data == _diamond)
                ActionDispatcher.Dispatch(new AddDiamondsToServerAction(amount));

            if (loot.Item.Data == _soul)
                ActionDispatcher.Dispatch(new AddSouls(amount));
        }

        public void Visit(EquipmentLoot loot)
        {
            Debug.LogWarning($"Try to loot equipment {loot.EquipmentId} but haven't implemented yet");
            ActionDispatcher.Dispatch(new AddEquipmentRequestAction(loot.EquipmentId));
        }

        public void Visit(MagicStoneLoot loot)
        {
            ActionDispatcher.Dispatch(new AddRewardedMagicStoneAction(loot.StoneId, loot.Quantity));
        }

        public void Visit(ExpLoot loot)
        {
            ActionDispatcher.Dispatch(new AddExpToPartyAction(loot.Exp));
            foreach (var slot in _partyManager.Slots)
            {
                if (!slot.HeroBehaviour.IsValidAndAlive()) continue;
                var expProvider = slot.HeroBehaviour.GetComponent<IExpProvider>();
                ActionDispatcher.Dispatch(new UpdateCharacterExpAction(slot.HeroBehaviour.Spec.Id, expProvider.Exp));
            }
        }
    }
}