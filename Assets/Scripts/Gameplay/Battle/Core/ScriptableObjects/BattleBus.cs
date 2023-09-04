using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        public EnemyParty CurrentEnemyParty { get; set; }
    }
}