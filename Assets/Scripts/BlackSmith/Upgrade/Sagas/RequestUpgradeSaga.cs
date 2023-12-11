using System;
using System.Net;
using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using UniRx;
using CryptoQuest.UI.Popups;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class RequestUpgradeSaga : SagaBase<RequestUpgrade>
    {
        [Serializable]
        public struct RequestUpgradeBody
        {
            [JsonProperty("beforeLv")]
            public int BeforeLevel;

            [JsonProperty("equipment")]
            public EquipmentRequestData Equipment;
        }

        [Serializable]
        public struct EquipmentRequestData
        {
            [JsonProperty("id")]
            public uint Id;

            [JsonProperty("equipmentId")]
            public string EquipmentId;

            [JsonProperty("lv")]
            public int Lv;
        }

        private const string UPGRADE_API = "crypto/equipments/upgrade"; 

        private UpgradeResponse _response;
        private IRestClient _restClient;

        protected override void HandleAction(RequestUpgrade ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restClient ??= ServiceProvider.GetService<IRestClient>();
            var equipment = ctx.EquipmentToUpgrade;

            var requestBody = new RequestUpgradeBody()
            {
                BeforeLevel = equipment.Level,
                Equipment = new EquipmentRequestData()
                {
                    Id = equipment.Id,
                    Lv = ctx.UpgradeLevel,
                    EquipmentId = equipment.Data.ID
                }
            };
            
            _restClient.WithBody(requestBody)
                .Post<UpgradeResponse>(UPGRADE_API)
                .Subscribe(GotResponse, DispatchUpgradeFailed, DispatchUpgradeFinished);
        }

        private void GotResponse(UpgradeResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            _response = response;
        }
        
        private void DispatchUpgradeFailed(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
            ActionDispatcher.Dispatch(new UpgradeFailed());
        }

        private void DispatchUpgradeFinished()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new UpgradeResponsed(_response));
        }
    }
}