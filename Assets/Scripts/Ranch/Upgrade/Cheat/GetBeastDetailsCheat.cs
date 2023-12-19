using System;
using System.Net;
using CommandTerminal;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Cheat
{
    public class GetBeastDetailsCheat : MonoBehaviour, ICheatInitializer
    {
        private const string GET_BEAST_DETAILS = "crypto/beasts";
        private IRestClient _restAPINetworkController;

        public void InitCheats()
        {
            Debug.Log("GetBeastDetailsCheat::InitCheats()");
            Terminal.Shell.AddCommand("get.beasts", GetDetailsHandler, 0, 0, "Get Beasts details");
        }

        private void GetDetailsHandler(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .Get<BeastsResponse>(GET_BEAST_DETAILS)
                .Subscribe(OnHandlerSucceed, OnHandlerFailed);
        }

        private void OnHandlerSucceed(BeastsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"Get Beast Details:: Success");
            GetDetails(response);
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnHandlerFailed(Exception obj)
        {
            Debug.Log($"Get Beast Details:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void GetDetails(BeastsResponse response)
        {
            foreach (var data in response.data.beasts)
            {
                Debug.Log($"Id: {data.id} / Beast Id: {data.beastId} / Level: {data.level} / Max lv: {data.maxLv}");
            }
        }
    }
}