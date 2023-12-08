using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class FetchNftEquipmentsSaga : SagaBase<FetchNftEquipments>
    {
        protected override void HandleAction(FetchNftEquipments ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParam("nft", "1")
                .Get<EquipmentsResponse>(Profile.EQUIPMENTS)
                .Subscribe(ProcessResponseEquipments, OnError, OnCompleted);
        }

        private void ProcessResponseEquipments(EquipmentsResponse nftEquipmentsResponse)
        {
            if (nftEquipmentsResponse.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new FetchNftEquipmentSucceed(nftEquipmentsResponse.data.equipments));
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnError(Exception obj) { }
    }
}