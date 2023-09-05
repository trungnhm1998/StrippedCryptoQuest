using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        public virtual EnemyParty CurrentEnemyParty
        {
            get => _currentEnemyParty;
            set => _currentEnemyParty = value;
        }

        private EnemyParty _currentEnemyParty;
    }
}