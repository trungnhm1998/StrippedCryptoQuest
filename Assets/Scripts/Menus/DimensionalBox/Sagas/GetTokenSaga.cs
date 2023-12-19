using System;
using System.Net;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using CryptoQuest.UI.Actions;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class GetTokenSaga : SagaBase<GetToken>
    {
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _ingameMetad;
        [SerializeField] private CurrencySO _inboxMetad;
        
        protected override void HandleAction(GetToken ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Get<GetTokenResponse>(Nft.GET_TOKEN)
                .Subscribe(OnGetToken, OnError);
        }

        private void OnGetToken(GetTokenResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;

            ActionDispatcher.Dispatch(new ShowLoading(false));
            _wallet[_ingameMetad].SetAmount(response.data.diamond);
            _wallet[_inboxMetad].SetAmount(response.data.metad);
            ActionDispatcher.Dispatch(new GetTokenSuccess());
        }

        private void OnError(Exception response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
            ActionDispatcher.Dispatch(new GetTokenFailed());
        }
    }
}