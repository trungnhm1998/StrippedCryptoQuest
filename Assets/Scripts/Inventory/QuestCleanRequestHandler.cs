using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Quest;
using CryptoQuest.Quest.Sagas;
using CryptoQuest.System.SaveSystem;
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
        public float diamond;
        public int soul;
        public long time;
    }

    public class QuestCleanRequestHandler : CoSagaBase<QuestCleanAllAction>
    {
        [SerializeField] private QuestSaveSO _saveData;
        [SerializeField] private SaveSystemSO _saveSystem;

        protected override IEnumerator HandleActionCoroutine(QuestCleanAllAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();

            var op = restClient
                .WithoutDispactError()
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Request<QuestCleanResponse>(ERequestMethod.DELETE, Quests.QUEST_DEBUG_CLEAN_ALL)
                .ToYieldInstruction();

            yield return op;

            _saveSystem.SaveData.Objects.RemoveAll(keyValue => keyValue.Key == "QuestSave");
            _saveData.ClearAll();

            Debug.Log($"<color=green>QuestCleanRequestHandler.OnSucceed</color> {op.Result.message}");
        }
    }
}