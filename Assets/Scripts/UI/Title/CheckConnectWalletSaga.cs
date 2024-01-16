using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;

namespace CryptoQuest.UI.Title
{
    public class CheckConnectWallet : ActionBase { }
    public class ConnectedWallet : ActionBase { }
    public class ConnectedWalletFailed : ActionBase { }

    public class CheckConnectWalletSaga : SagaBase<CheckConnectWallet>
    {
        protected override void HandleAction(CheckConnectWallet ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithoutDispactError()
                .Get<CommonResponse>(Nft.GET_TOKEN)
                .Subscribe(OnGetToken, OnError);
        }

        private void OnGetToken(CommonResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ConnectedWallet());
        }

        private void OnError(Exception response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ConnectedWalletFailed());
        }
    }
}