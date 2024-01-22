using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferringMetadSaga : SagaBase<TransferringMetad>
    {
        [SerializeField] private CurrencySO _ingameCurrency;
        [SerializeField] private CurrencySO _inboxCurrency;
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private MetadTransferPanel _metaDTransferPanel;

        protected override void HandleAction(TransferringMetad ctx)
        {
            var apiPath = ctx.SourceToTransfer == _ingameCurrency
                ? Nft.TRANSFER_METAD_TO_WALLET
                : Nft.TRANSFER_METAD_TO_GAME;
            TransferToWallet(apiPath, ctx.Amount);
        }

        private void TransferToWallet(string apiPath, float amount)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            ServiceProvider.GetService<IRestClient>()
                .WithBody(new Dictionary<string, float> { { "amount", amount } })
                .Post<GetTokenResponse>(apiPath)
                .Subscribe(TransferSucceed, TransferFailed, HideLoading);
        }

        private void TransferSucceed(GetTokenResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK)
            {
                TransferFailed(new Exception(response.message));
                return;
            }

            _wallet[_ingameCurrency].SetAmount(response.diamond);
            _wallet[_inboxCurrency].SetAmount(response.data.metad);

            ActionDispatcher.Dispatch(new TransferringMetadSuccess());
        }

        private void TransferFailed(Exception response)
        {
            HideLoading();
            Debug.LogWarning($"TransferringMetadSaga::TransferFailed [{response}]");
            ActionDispatcher.Dispatch(new TransferringMetadFailed());
        }

        private void HideLoading()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}