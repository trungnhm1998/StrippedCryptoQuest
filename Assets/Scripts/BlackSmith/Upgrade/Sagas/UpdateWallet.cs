using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class UpdateWallet : SagaBase<UpgradeSucceed>
    {
        [SerializeField] private WalletSO _walletSO;
        [SerializeField] private CurrencySO _currencySO;

        protected override void HandleAction(UpgradeSucceed ctx)
        {
            _walletSO[_currencySO].SetAmount(ctx.GoldAfter);
        }
    }
}