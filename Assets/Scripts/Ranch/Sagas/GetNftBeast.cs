using System;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Beast;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    public class GetNftBeast : SagaBase<FetchProfileBeastsAction>
    {
        [SerializeField] private BeastInventorySO _beastInventory;

        protected override void HandleAction(FetchProfileBeastsAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<BeastsResponse>(BeastAPI.GET_BEASTS)
                .Subscribe(OnGetBeasts, OnError);
        }

        private void OnGetBeasts(BeastsResponse response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (response.code != (int)HttpStatusCode.OK) return;

            var beastResponses = response.data.beasts;
            if (beastResponses.Length == 0) return;

            UpdateBeastsByStatus(beastResponses);

            ActionDispatcher.Dispatch(new GetBeastSucceeded());
        }

        private void UpdateBeastsByStatus(BeastResponse[] data)
        {
            var inBox = data.Where(x => x.inGameStatus == (int)EBeastStatus.InBox).ToArray();
            ActionDispatcher.Dispatch(new FetchInboxBeastSucceeded(inBox));

            var inGame = data.Where(x => x.inGameStatus == (int)EBeastStatus.InGame).ToArray();
            ActionDispatcher.Dispatch(new FetchInGameBeastSucceeded(inGame));

            FillInventory(inGame);
        }

        private void FillInventory(BeastResponse[] data)
        {
            var converter = ServiceProvider.GetService<IBeastResponseConverter>();

            _beastInventory.OwnedBeasts.Clear();

            foreach (var beastResponse in data)
            {
                if (beastResponse.id == -1) continue;
                _beastInventory.OwnedBeasts.Add(converter.Convert(beastResponse));
            }

            ActionDispatcher.Dispatch(new BeastInventoryFilled());
        }

        private void OnError(Exception ex)
        {
            Debug.Log($"<color=white>Saga::GetNftBeast::Error</color>:: {ex}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetBeastsFailed());
        }
    }
}