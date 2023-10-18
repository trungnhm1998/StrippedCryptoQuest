using CryptoQuest.Gameplay.Encounter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        [field: SerializeField] public Battlefield CurrentBattlefield { get; set; }
        [field: SerializeField] public Scene LastActiveScene { get; set; }
    }
}