using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIChangeClass = CryptoQuest.API.ChangeClass;

namespace CryptoQuest.ChangeClass.API
{
    public class WalletCharacterAPI : MonoBehaviour
    {
        private IRestClient _restAPINetworkController;
        public bool IsFinishFetchData { get; private set; }

        public List<CharacterAPI> Data { get; private set; } = new();

        public IEnumerator LoadCharacterFromWallet()
        {
            IsFinishFetchData = false;
            Data.Clear();
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .Get<CharacterResponseData>(APIChangeClass.LOAD_ALL_CHARACTER)
                .ToYieldInstruction();
            yield return op;
            IsFinishFetchData = true;
            Data = op.Result.data.characters;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}