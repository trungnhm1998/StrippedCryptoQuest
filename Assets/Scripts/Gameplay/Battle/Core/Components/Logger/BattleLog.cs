using System.Collections.Generic;
using CryptoQuest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.Logger
{
    public class BattleLog : MonoBehaviour, ILogger
    {
        [SerializeField] private BattleActionDataEventChannelSO _gotActionDataEventChannel;
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;

        private List<string> _logs = new();

        private void OnEnable()
        {
            _gotActionDataEventChannel.EventRaised += OnGotActionData;
        }

        private void OnDisable()
        {
            _gotActionDataEventChannel.EventRaised -= OnGotActionData;
        }

        private void OnGotActionData(BattleActionDataSO data)
        {
            var log = data.Log.GetLocalizedString();
            Log(log);
        }

        /// <summary>
        /// Override this if you want to save all battle logs
        /// </summary>
        /// <param name="message"></param>
        public virtual void Log(string message)
        {
            _gotNewLogEventChannel.RaiseEvent(message);
            _logs.Add(message);
        }

        public virtual void ClearLogs()
        {
            _logs.Clear();
        }
    }
}
