using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class WalletManager : MonoBehaviour
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _diamonds;
        [SerializeField] private CurrencySO _soul;

        private TinyMessageSubscriptionToken _addGoldEvent;
        private TinyMessageSubscriptionToken _removeGoldEvent;
        private TinyMessageSubscriptionToken _addDiamondsEvent;
        private TinyMessageSubscriptionToken _removeDiamondsEvent;
        private TinyMessageSubscriptionToken _addSoulEvent;
        private TinyMessageSubscriptionToken _removeSoulEvent;
        private TinyMessageSubscriptionToken _setGoldEvent;
        private TinyMessageSubscriptionToken _setDiamondEvent;
        private TinyMessageSubscriptionToken _setSoulEvent;

        private void OnEnable()
        {
            _addGoldEvent = ActionDispatcher.Bind<AddGoldAction>(AddGold);
            _removeGoldEvent = ActionDispatcher.Bind<RemoveGoldAction>(RemoveGold);

            _addDiamondsEvent = ActionDispatcher.Bind<AddDiamonds>(AddDiamonds);
            _removeDiamondsEvent = ActionDispatcher.Bind<RemoveDiamonds>(RemoveDiamonds);

            _addSoulEvent = ActionDispatcher.Bind<AddSouls>(AddSouls);
            _removeSoulEvent = ActionDispatcher.Bind<RemoveSouls>(RemoveSouls);

            _setGoldEvent = ActionDispatcher.Bind<SetGoldAction>(SetGold);
            _setDiamondEvent = ActionDispatcher.Bind<SetDiamondAction>(SetDiamond);
            _setSoulEvent = ActionDispatcher.Bind<SetSoulAction>(SetSoul);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addGoldEvent);
            ActionDispatcher.Unbind(_removeGoldEvent);

            ActionDispatcher.Unbind(_addDiamondsEvent);
            ActionDispatcher.Unbind(_removeDiamondsEvent);

            ActionDispatcher.Unbind(_addSoulEvent);
            ActionDispatcher.Unbind(_removeSoulEvent);

            ActionDispatcher.Unbind(_setGoldEvent);
            ActionDispatcher.Unbind(_setDiamondEvent);
            ActionDispatcher.Unbind(_setSoulEvent);
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

        private void AddSouls(AddSouls ctx)
        {
            var amount = ctx.Amount;
            _wallet[_soul].Amount += amount;
        }

        private void RemoveSouls(RemoveSouls ctx)
        {
            var amount = ctx.Amount;
            _wallet[_soul].Amount -= amount;
        }


        private void SetGold(SetGoldAction ctx)
        {
            var amount = ctx.Amount;
            _wallet[_gold].Amount = amount;
        }

        private void SetDiamond(SetDiamondAction ctx)
        {
            var amount = ctx.Amount;
            _wallet[_diamonds].Amount = amount;
        }

        private void SetSoul(SetSoulAction ctx)
        {
            var amount = ctx.Amount;
            _wallet[_soul].Amount = amount;
        }
    }
}