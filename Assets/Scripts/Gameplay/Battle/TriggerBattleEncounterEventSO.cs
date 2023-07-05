using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle
{
    public class TriggerBattleEncounterEventSO : ScriptableObject
    {
        public UnityAction<BattleDataSO> EncounterBattle;
    }
}