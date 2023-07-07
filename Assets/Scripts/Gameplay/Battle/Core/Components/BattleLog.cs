using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLog : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _gotNewLogEventChannel;

        /// <summary>
        /// Override this if you want to save all battle logs
        /// </summary>
        /// <param name="data"></param>
        public virtual void Log(BattleLogData data)
        {
            Debug.Log($"BattleLog::Log {data.Message}");
            _gotNewLogEventChannel.RaiseEvent(data.Message);
        }
    }

    public struct BattleLogData
    {
        public string Message;
    }

}
