using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class CurrencyAmountChangedAction : ActionBase
    {
        public int Amount { get; }

        public CurrencyAmountChangedAction(int amount)
        {
            Amount = amount;
        }
    }

    public class AddGoldAction : CurrencyAmountChangedAction
    {
        public AddGoldAction(int amount) : base(amount) { }
    }

    public class RemoveGoldAction : CurrencyAmountChangedAction
    {
        public RemoveGoldAction(int amount) : base(amount) { }
    }

    public class AddDiamonds : CurrencyAmountChangedAction
    {
        public AddDiamonds(int amount) : base(amount) { }
    }

    public class RemoveDiamonds : CurrencyAmountChangedAction
    {
        public RemoveDiamonds(int amount) : base(amount) { }
    }

    public class WalletManager : MonoBehaviour
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _diamonds;
        private TinyMessageSubscriptionToken _addGoldEvent;
        private TinyMessageSubscriptionToken _removeGoldEvent;
        private TinyMessageSubscriptionToken _addDiamondsEvent;
        private TinyMessageSubscriptionToken _removeDiamondsEvent;

        private void OnEnable()
        {
            _addGoldEvent = ActionDispatcher.Bind<AddGoldAction>(AddGold);
            _removeGoldEvent = ActionDispatcher.Bind<RemoveGoldAction>(RemoveGold);

            _addDiamondsEvent = ActionDispatcher.Bind<AddDiamonds>(AddDiamonds);
            _removeDiamondsEvent = ActionDispatcher.Bind<RemoveDiamonds>(RemoveDiamonds);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addGoldEvent);
            ActionDispatcher.Unbind(_removeGoldEvent);

            ActionDispatcher.Unbind(_addDiamondsEvent);
            ActionDispatcher.Unbind(_removeDiamondsEvent);
        }

        private void AddGold(AddGoldAction ctx)
        {
            var amount = ctx.Amount;
            _wallet[_gold].Amount += amount;
        }

        private void RemoveGold(RemoveGoldAction ctx)
        {
            var amount = ctx.Amount;
            _wallet[_gold].Amount -= amount;
        }

        private void AddDiamonds(AddDiamonds ctx)
        {
            var amount = ctx.Amount;
            _wallet[_diamonds].Amount += amount;
        }

        private void RemoveDiamonds(RemoveDiamonds ctx)
        {
            var amount = ctx.Amount;
            _wallet[_diamonds].Amount -= amount;
        }
    }
}