using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class GetMagicStones : SagaBase<GetNftMagicStone>
    {
        [SerializeField] private GetMagicStonesEvent _inGameEvent;
        [SerializeField] private GetMagicStonesEvent _inBoxEvent;

        private readonly List<MagicStone> _inGameStonesCache = new();
        private readonly List<MagicStone> _inBoxStonesCache = new();

        protected override void HandleAction(GetNftMagicStone ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>() { { "source", $"{((int)ctx.Status).ToString()}" } })
                .Get<MagicStonesResponse>(Profile.MAGIC_STONE)
                .Subscribe(OnGetMagicStones, OnError);
        }

        private void OnError(Exception ex)
        {
            Debug.Log($"GetMagicStones::OnError: {ex}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftMagicStoneFailed());
        }

        private void OnGetMagicStones(MagicStonesResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.stones);
            UpdateInboxCache(response.data.stones);

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftMagicStoneSucceed());
        }

        private void UpdateInGameCache(MagicStone[] stones)
        {
            if (stones.Length == 0) return;
            _inGameStonesCache.Clear();
            foreach (var stone in stones)
            {
                if (stone.inGameStatus != (int)EMagicStoneStatus.InGame) continue;
                _inGameStonesCache.Add(stone);
            }

            _inGameEvent.RaiseEvent(_inGameStonesCache);
            Debug.Log($"GetMagicStones::UpdateInGameCache: {_inGameStonesCache.Count}");
        }

        private void UpdateInboxCache(MagicStone[] stones)
        {
            if (stones.Length == 0) return;
            _inBoxStonesCache.Clear();
            foreach (var stone in stones)
            {
                if (stone.inGameStatus != (int)EMagicStoneStatus.InBox) continue;
                _inBoxStonesCache.Add(stone);
            }

            _inBoxEvent.RaiseEvent(_inBoxStonesCache);
            Debug.Log($"GetMagicStones::UpdateInboxCache: {_inBoxStonesCache.Count}");
        }
    }
}