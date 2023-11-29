using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Character.Beast;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UniRx;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;


namespace CryptoQuest.Ranch.Sagas
{
    public class FetchProfileBeast : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileBeastAction>(HandleAction);
            _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
        }

        private void FilterAndRefreshInventory(TransferSucceed ctx)
        {
            var ingameBeasts = ctx.ResponseBeasts.Where(beast => beast.inGameStatus == (int)EBeastStatus.InGame)
                .ToList();
            OnInventoryFilled(ingameBeasts.ToArray());
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
            ActionDispatcher.Unbind(_transferSuccessEvent);
        }

        private void HandleAction(FetchProfileBeastAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();

            restClient
                .WithParams(new Dictionary<string, string>
                    { { "source", $"{((int)EBeastStatus.InGame).ToString()}" } })
                .Get<BeastsResponse>(Profile.GET_BEASTS)
                .Subscribe(ProcessResponseBeasts, OnError);
        }

        private void ProcessResponseBeasts(BeastsResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseBeasts = obj.data.beasts;
            if (responseBeasts.Length == 0) return;

            OnInventoryFilled(responseBeasts);
        }

        private void OnInventoryFilled(Obj.Beast[] beasts) => StartCoroutine(CoLoadAndUpdateInventory(beasts));

        private IEnumerator CoLoadAndUpdateInventory(Obj.Beast[] beasts)
        {
            yield return null;
            
            var nftBeast = beasts.Select(CreateNftBeast).ToList();
            Debug.Log($"CoLoadAndUpdateInventory: {nftBeast}");
        }

        private BeastSpec CreateNftBeast(Obj.Beast beast)
        {
            var nftBeast = new BeastSpec();
            Debug.Log($"CreateNftBeast: {beast}");
            return nftBeast;
        }

        private void OnError(Exception error)
        {
            Debug.LogError($"FetchProfileBeast::OnError: {error}");
        }
    }
}