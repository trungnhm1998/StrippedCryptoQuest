using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle
{
    public class TriggerBattleEncounterEventSO : ScriptableObject
    {
        public UnityAction<BattleDataSO> EncounterBattle;

        public void Raise(BattleDataSO battleData)
        {
            EncounterBattle.Invoke(battleData);
        }
    }
}