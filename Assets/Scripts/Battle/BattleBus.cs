using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Battle
{
    public class BattleBus : ScriptableObject
    {
        [field: SerializeField] public EncounterData CurrentEncounter { get; set; }
        [field: SerializeField] public Battlefield CurrentBattlefield { get; set; }
        [field: SerializeField] public Scene LastActiveScene { get; set; }
        public BattleContext CurrentBattleContext { get; set; }
    }
}