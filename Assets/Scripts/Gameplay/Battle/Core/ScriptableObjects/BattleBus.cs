using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleBus", menuName = "Gameplay/Battle/Battle Bus")]
    public class BattleBus : ScriptableObject
    {
        public BattleManager BattleManager { get; set; }
    }
}