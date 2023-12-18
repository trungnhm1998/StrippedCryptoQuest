using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferringMagicStonesSaga : SagaBase<TransferringMagicStones>
    {
        protected override void HandleAction(TransferringMagicStones ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new Dictionary<string, List<int>>()
                {
                    { "game", ctx.ToGame.Select(item => item.Id).ToList() },
                    { "wallet", ctx.ToWallet.Select(item => item.Id).ToList() }
                })
                .Put<MagicStonesResponse>(MagicStoneAPI.TRANSFER)
                .Subscribe(ProcessResponseMagicStones, OnError, OnCompleted);
        }

        private void ProcessResponseMagicStones(MagicStonesResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new FetchNftMagicStonesSucceed(response.data.stones));
            ActionDispatcher.Dispatch(new TransferSucceed());
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}