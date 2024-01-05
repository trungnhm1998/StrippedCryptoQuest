using System;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.MagicStone
{
    public class GetMagicStones : SagaBase<FetchProfileMagicStonesAction>
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;

        protected override void HandleAction(FetchProfileMagicStonesAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<MagicStonesResponse>(MagicStoneAPI.GET_MAGIC_STONE)
                .Subscribe(ProcessResponseMagicStone, OnError);
        }

        private void ProcessResponseMagicStone(MagicStonesResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseStones = obj.data.stones;
            ActionDispatcher.Dispatch<GetStonesResponsed>(new GetStonesResponsed(obj));
            if (responseStones.Length == 0) return;

            FilterStonesByStatus(responseStones);
        }

        private void FilterStonesByStatus(Objects.MagicStone[] stonesResponse)
        {
            if (stonesResponse.Length == 0) return;

            var dboxStones = stonesResponse.Where(stone => stone.inGameStatus == (int)Objects.EMagicStoneStatus.InBox).ToArray();
            ActionDispatcher.Dispatch(new FetchInboxMagicStonesSuccess(dboxStones));

            var inGameStones = stonesResponse.Where(stone => stone.inGameStatus == (int)Objects.EMagicStoneStatus.InGame).ToArray();
            ActionDispatcher.Dispatch(new FetchIngameMagicStonesSuccess(inGameStones));
            FillInventory(inGameStones);
        }

        private void FillInventory(Objects.MagicStone[] stonesResponse)
        {
            _stoneInventory.MagicStones.Clear();
            var converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var stoneResponse in stonesResponse)
            {
                if (stoneResponse.id == -1) continue;
                _stoneInventory.MagicStones.Add(converter.Convert(stoneResponse));
            }

            ActionDispatcher.Dispatch(new StoneInventoryFilled());
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::GetMagicStones::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetStonesFailed());
        }
    }
}