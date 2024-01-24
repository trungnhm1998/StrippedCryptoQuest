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
    public class WalletMaterialAPI : MonoBehaviour
    {
        private IRestClient _restAPINetworkController;

        public List<MaterialAPI> Data { get; private set; } = new();
        public bool IsFinishFetchData { get; private set; }

        public IEnumerator LoadMaterialsFromWallet()
        {
            Data.Clear();
            IsFinishFetchData = false;
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .Get<MaterialResponseData>(APIChangeClass.LOAD_MATERIAL)
                .ToYieldInstruction();
            yield return op;
            Data = op.Result.data.materials;
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}