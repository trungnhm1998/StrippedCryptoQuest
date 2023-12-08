﻿using System;
using System.Collections.Generic;
using System.Net;
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
                .WithBody(new Dictionary<string, List<uint>>()
                {
                    { "game", ctx.ToGame },
                    { "wallet", ctx.ToWallet }
                })
                .Put<EquipmentsResponse>(API.TRANSFER)
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
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}