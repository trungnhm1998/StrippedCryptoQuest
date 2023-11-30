using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferMagicStoneToBothGameAndBox : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendMagicStoneToBothSideEvent;

        private void OnEnable()
        {
            _sendMagicStoneToBothSideEvent = ActionDispatcher.Bind<SendMagicStoneToBothSide>(CallAPI);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendMagicStoneToBothSideEvent);
        }

        private void CallAPI(SendMagicStoneToBothSide obj)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "game", obj.SelectedInGameMagicStones },
                { "wallet", obj.SelectedInDboxMagicStones }
            };

            Debug.Log($"SendMagicStoneToBothSide::Body={JsonUtility.ToJson(body)}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<MagicStonesResponse>(Profile.PUT_MAGIC_STONE_TO_BOX_AND_GAME)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));

        private void OnError(Exception obj)
        {
            Debug.LogError("TransferMagicStoneToBothSideFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferMagicStoneFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnNext(MagicStonesResponse obj)
        {
            if (obj.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferMagicStoneSucceed(obj.data.stones));
        }
    }
}