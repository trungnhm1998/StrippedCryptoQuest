using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UniRx;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch.Sagas
{
    public class GetNftBeast : SagaBase<GetBeasts>
    {
        private readonly List<Obj.Beast> _inGameBeasts = new();
        private readonly List<Obj.Beast> _walletBeasts = new();

        protected override void HandleAction(GetBeasts ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>() { { "source", $"{((int)ctx.Status).ToString()}" } })
                .Get<Obj.BeastsResponse>(Profile.GET_BEASTS)
                .Subscribe(OnGetBeasts, OnError);
        }

        private void OnError(Exception ex)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.Log($"GetNftBeast::OnError: {ex}");
            ActionDispatcher.Dispatch(new GetNftBeastsFailed());
        }

        private void OnGetBeasts(Obj.BeastsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.beasts);
            UpdateInboxCache(response.data.beasts);

            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void UpdateInGameCache(Obj.Beast[] dataBeasts)
        {
            if (dataBeasts.Length == 0) return;
            _inGameBeasts.Clear();

            foreach (var beast in dataBeasts)
            {
                if (beast.inGameStatus != (int)Obj.EBeastStatus.InGame) continue;
                _inGameBeasts.Add(beast);
            }

            ActionDispatcher.Dispatch(new GetGameNftBeastsSucceed(_inGameBeasts));
        }

        private void UpdateInboxCache(Obj.Beast[] dataBeasts)
        {
            if (dataBeasts.Length == 0) return;
            _walletBeasts.Clear();

            foreach (var beast in dataBeasts)
            {
                if (beast.inGameStatus != (int)Obj.EBeastStatus.InBox) continue;
                _walletBeasts.Add(beast);
            }

            ActionDispatcher.Dispatch(new GetWalletNftBeastsSucceed(_walletBeasts));
        }
    }
}