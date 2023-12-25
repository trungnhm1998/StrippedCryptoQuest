using System;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Quest.Sagas
{
    public class LogsBody
    {
        [JsonProperty("logs")]
        public EventLog[] Logs;
    }

    public class EventLog
    {
        [JsonProperty("questId")]
        public string QuestName;
    }

    public class UpdateEventLogs : SagaBase<LogEventAction>
    {
        protected override void HandleAction(LogEventAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            var body = new LogsBody()
            {
                Logs = new[]
                {
                    new EventLog()
                    {
                        QuestName = ctx.QuestName,
                    }
                }
            };
            restClient
                .WithBody(body)
                .Post<EventLogResponse>(API.Events.LOG)
                .Subscribe(ProcessResponse, OnError, OnCompleted);
        }

        private void ProcessResponse(EventLogResponse nftEquipmentsResponse)
        {
            Debug.Log($"<color=white>Saga::UpdateEventLogs::ProcessResponse</color>:: {nftEquipmentsResponse}");
        }

        private void OnCompleted()
        {
            Debug.Log($"<color=white>Saga::UpdateEventLogs::Completed</color>");
        }

        private void OnError(Exception obj)
        {
            Debug.Log($"<color=white>Saga::UpdateEventLogs::Error</color>:: {obj}");
        }
    }
}