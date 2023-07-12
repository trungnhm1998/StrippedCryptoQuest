using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleUnitLogDebug : BattleUnitLog
    {
        public override void Log(string message)
        {
            base.Log(message);
            Debug.Log($"BattleUnitLogDebug:: {message}");
        }
    }
}
