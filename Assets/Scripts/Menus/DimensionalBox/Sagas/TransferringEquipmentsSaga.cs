using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferringEquipmentsSaga : SagaBase<TransferringEquipments>
    {
        protected override void HandleAction(TransferringEquipments ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParam("nft", "1")
                .WithBody(new Dictionary<string, List<int>>()
                {
                    { "game", ctx.ToGame.Select(item => item.Id).ToList() },
                    { "wallet", ctx.ToWallet.Select(item => item.Id).ToList() }
                })
                .Put<EquipmentsResponse>(EquipmentAPI.TRANSFER)
                .Subscribe(ProcessResponseEquipments, OnError, OnCompleted);
        }

        private void ProcessResponseEquipments(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new FetchNftEquipmentSucceed(response.data.equipments));
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