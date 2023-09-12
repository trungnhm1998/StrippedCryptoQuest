using System;
using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        public Battlefield CurrentBattlefield
        {
            get
            {
                if (_currentBattlefield == null)
                    _currentBattlefield = CreateInstance<Battlefield>();
                return _currentBattlefield;
            }
            set => _currentBattlefield = value;
        }

        [NonSerialized] private Battlefield _currentBattlefield = null;
    }
}