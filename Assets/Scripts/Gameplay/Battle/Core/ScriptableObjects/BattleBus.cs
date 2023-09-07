using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        public virtual Battlefield CurrentBattlefield
        {
            get => _currentBattlefield;
            set => _currentBattlefield = value;
        }

        private Battlefield _currentBattlefield;
    }
}