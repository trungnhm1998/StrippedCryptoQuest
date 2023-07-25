using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.Logger
{
    public class BattleLogDebug : BattleLog
    {
        public override void Log(string message)
        {
            base.Log(message);
            Debug.Log($"BattleUnitLogDebug:: {message}");
        }
    }
}
