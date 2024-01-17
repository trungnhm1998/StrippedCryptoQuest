using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.Sagas
{
    public class UpdateNftBeast : SagaBase<UpdateBeastEquippedAction>
    {
        [SerializeField] private BeastProvider _beastProvider;

        protected override void HandleAction(UpdateBeastEquippedAction ctx)
        {
            var body = new Dictionary<string, int>()
            {
                { "id", ctx.BeastId },
                { "equip", _beastProvider.EquippingBeast.Id == ctx.BeastId ? 1 : 0 }
            };

            Debug.Log($"<color=green>UpdateNftBeast::Body={JsonConvert.SerializeObject(body)}</color>");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<BeastsResponse>(BeastAPI.EQUIP_BEAST)
                .Subscribe(OnSucceeded, OnError);
        }

        private void OnSucceeded(BeastsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"<color=green>UpdateNftBeast::Response={JsonConvert.SerializeObject(response)}</color>");
        }

        private void OnError(Exception ex)
        {
            Debug.LogWarning($"<color=red> UpdateNftBeast::Error={ex}</color>");
        }
    }
}