using System;
using System.Net;
using CommandTerminal;
using CryptoQuest.BlackSmith.Upgrade.Sagas;
using CryptoQuest.Networking;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.Cheat
{
    public class ModifyEquipmentLevelCheat : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("id")]
            public int Id;

            [JsonProperty("lv")]
            public int Level;
        }

        private const string UPGRADE_API = "crypto/equipments";

        private IRestClient _restAPINetworkController;

        public void InitCheats()
        {
            Debug.Log("ModifyLevelCheat::InitCheats()");
            Terminal.Shell.AddCommand("upgrade.equipment", ModifyLevelHandler, 2, 2, "[Id] [Level]");
        }

        private void ModifyLevelHandler(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { Id = args[0].Int, Level = args[1].Int })
                .Put<UpgradeResponse>(UPGRADE_API)
                .Subscribe(OnModifySucceed, OnModifyFailed);
        }

        private void OnModifySucceed(UpgradeResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"Upgrade Beast:: Success");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnModifyFailed(Exception obj)
        {
            Debug.Log($"Upgrade Beast:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}