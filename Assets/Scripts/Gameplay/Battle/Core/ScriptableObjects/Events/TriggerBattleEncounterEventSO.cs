using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events
{
    public class TriggerBattleEncounterEventSO : ScriptableObject
    {
        public UnityAction<BattleInfo> EncounterBattle;

        public void Raise(BattleInfo battleInfo)
        {
            if (EncounterBattle == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EncounterBattle.Invoke(battleInfo);
        }
    }
}