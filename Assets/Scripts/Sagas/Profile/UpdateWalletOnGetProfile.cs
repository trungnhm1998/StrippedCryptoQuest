using CryptoQuest.Actions;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using UnityEngine;

namespace CryptoQuest.Sagas.Profile
{
    public class UpdateWalletOnGetProfile : SagaBase<UpdateWallet>
    {
        [SerializeField] private Credentials _credentials;
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _soul;
        [SerializeField] private CurrencySO _metad;

        protected override void HandleAction(UpdateWallet ctx)
        {
            _credentials.Profile.user.walletAddress = ctx.WalletAddress;
            _wallet[_gold].SetAmount(ctx.Gold);
            _wallet[_soul].SetAmount(ctx.Soul);
            _wallet[_metad].SetAmount(ctx.Diamond);
        }
    }
}