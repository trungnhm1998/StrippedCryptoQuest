using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public interface ILogger
    {
        void Log(string message);
        void ClearLogs();
    }
}
