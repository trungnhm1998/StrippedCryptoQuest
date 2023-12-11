using System;
using System.Linq;
using System.Net;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.MagicStone
{
    public class GetMagicStones : MonoBehaviour
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;

        private TinyMessageSubscriptionToken _fetchEvent;

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileMagicStonesAction>(HandleAction);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
        }

        private void HandleAction(FetchProfileMagicStonesAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<MagicStonesResponse>(API.Profile.MAGIC_STONE)
                .Subscribe(ProcessResponseMagicStone, OnError);
        }

        private void ProcessResponseMagicStone(MagicStonesResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseStones = obj.data.stones;
            if (responseStones.Length == 0) return;

            FilterStonesByStatus(responseStones);

            ActionDispatcher.Dispatch<GetStonesResponsed>(new GetStonesResponsed(obj));
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
            _converter ??= ServiceProvider.GetService<IMagicStoneResponseConverter>();
            _stoneInventory.MagicStones.Clear();
            var converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var stoneResponse in stonesResponse)
            {
                if (stoneResponse.id == -1) continue;
                _stoneInventory.MagicStones.Add(_converter.Convert(stoneResponse));
            }

            ActionDispatcher.Dispatch(new StoneInventoryFilled());
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::GetMagicStones::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
            ActionDispatcher.Dispatch(new GetStonesFailed());
        }
    }
}