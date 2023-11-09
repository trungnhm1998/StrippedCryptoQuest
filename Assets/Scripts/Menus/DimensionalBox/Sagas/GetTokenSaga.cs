using System;
using System.Net;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Menus.DimensionalBox.Actions;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.Networking.API;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class GetTokenSaga : SagaBase<GetToken>
    {
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _ingameMetad;
        [SerializeField] private CurrencySO _inboxMetad;
        
        protected override void HandleAction(GetToken ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Get<GetTokenResponse>(Nft.GET_TOKEN)
                .Subscribe(OnGetToken, OnError);
        }

        private void OnGetToken(GetTokenResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            _wallet[_ingameMetad].SetAmount(response.data.diamond);
            _wallet[_inboxMetad].SetAmount(response.data.metad);
        }

        private void OnError(Exception response)
        {
            Debug.LogError($"GetTokenSaga: {response.Message}");
        }
    }
}