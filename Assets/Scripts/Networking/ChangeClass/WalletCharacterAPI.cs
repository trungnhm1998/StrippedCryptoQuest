using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Networking.API;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UniRx;

namespace CryptoQuest.Networking.ChangeClass
{
    public class WalletCharacterAPI : MonoBehaviour
    {
        private IRestClient _restAPINetworkController;

        public List<CharacterAPI> Data { get; private set; }
        public bool IsFinishFetchData { get; private set; }

        public void LoadCharacterFromWallet()
        {
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .Get<CharacterResponseData>(Characters.LOAD_ALL_CHARACTER)
                .Subscribe(Authenticated, DispatchLoadFailed, DispatchLoadFinished);
        }

        private void Authenticated(CharacterResponseData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            IsFinishFetchData = true;
            Data = response.data.characters;
        }

        private void DispatchLoadFailed(Exception obj)
        {
            ActionDispatcher.Dispatch(new LoginFailedAction());
            IsFinishFetchData = true;
        }

        private void DispatchLoadFinished()
        {
            ActionDispatcher.Dispatch(new LoginFinishedAction());
        }
    }
}
