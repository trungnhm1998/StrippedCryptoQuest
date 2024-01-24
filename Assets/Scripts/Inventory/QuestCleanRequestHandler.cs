using System;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Quest;
using CryptoQuest.Quest.Sagas;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    [Serializable]
    public class QuestCleanResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
    }

    public class QuestCleanRequestHandler : SagaBase<QuestCleanAllAction>
    {
        [SerializeField] private QuestSaveSO _saveData;

        protected override void HandleAction(QuestCleanAllAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithoutDispactError()
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Get<QuestCleanResponse>(Quests.QUEST_DEBUG_CLEAN_ALL)
                .Subscribe(OnSucceed, OnFailed);
        }

        private void OnSucceed(QuestCleanResponse response)
        {
            if (!response.success) return;

            _saveData.ClearAll();
            Debug.Log($"<color=green>QuestCleanRequestHandler.OnSucceed</color> {response.message}");
        }

        private void OnFailed(Exception e)
        {
            Debug.Log($"<color=red>QuestCleanRequestHandler.OnFailed</color> {e.Message}");
        }
    }
}