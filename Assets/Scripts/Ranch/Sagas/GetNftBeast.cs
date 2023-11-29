using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    public class GetNftBeast : SagaBase<GetBeasts>
    {
        private readonly List<BeastData> _inGameBeastsCache = new();
        private readonly List<BeastData> _inBoxBeastsCache = new();

        protected override void HandleAction(GetBeasts ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<BeastsResponse>(Profile.GET_BEASTS)
                .Subscribe(OnGetBeasts, OnError);
        }

        private void OnError(Exception ex)
        {
            Debug.Log($"GetNftBeast::OnError: {ex}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftBeastsFailed());
        }

        private void OnGetBeasts(BeastsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.beasts);
            UpdateInboxCache(response.data.beasts);

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftBeastsSucceed());
        }

        private void UpdateInGameCache(BeastData[] dataBeasts)
        {
            if (dataBeasts.Length == 0) return;
            _inGameBeastsCache.Clear();

            foreach (var beast in dataBeasts)
            {
                if (beast.inGameStatus != (int)EBeastStatus.InGame) continue;
                _inGameBeastsCache.Add(beast);
            }

            ActionDispatcher.Dispatch(new GetInGameBeastsSucceed(_inGameBeastsCache));
        }

        private void UpdateInboxCache(BeastData[] dataBeasts)
        {
            if (dataBeasts.Length == 0) return;
            _inBoxBeastsCache.Clear();

            foreach (var beast in dataBeasts)
            {
                if (beast.inGameStatus != (int)EBeastStatus.InBox) continue;
                _inBoxBeastsCache.Add(beast);
            }

            ActionDispatcher.Dispatch(new GetInBoxBeastsSucceed(_inBoxBeastsCache));
        }
    }
}