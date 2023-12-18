using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.Events;
using CryptoQuest.Sagas.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class UpdateWallet : SagaBase<GetStonesResponsed>
    {
        [SerializeField] private WalletSO _walletSO;
        [SerializeField] private CurrencySO _goldSO;
        [SerializeField] private CurrencySO _diamondSO;

        protected override void HandleAction(GetStonesResponsed ctx)
        {
            _walletSO[_goldSO].SetAmount(ctx.Response.gold);
            _walletSO[_diamondSO].SetAmount(ctx.Response.diamond);
        }
    }
}