using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleBus : ScriptableObject
    {
        public BattleManager BattleManager { get; set; }
    }
}