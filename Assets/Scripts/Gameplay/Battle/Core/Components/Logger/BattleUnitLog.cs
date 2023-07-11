using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleUnitLog : MonoBehaviour, ILogger
    {
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;
        [SerializeField] private VoidEventChannelSO _unitClearLogsEventChannel;

        private List<string> _logs = new();

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
            _unitClearLogsEventChannel.RaiseEvent();
            _logs.Clear();
        }
    }
}
