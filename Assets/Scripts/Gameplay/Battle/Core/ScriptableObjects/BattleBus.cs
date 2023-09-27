using CryptoQuest.Gameplay.Encounter;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        [field: SerializeField] public Battlefield CurrentBattlefield { get; set; }
    }
}