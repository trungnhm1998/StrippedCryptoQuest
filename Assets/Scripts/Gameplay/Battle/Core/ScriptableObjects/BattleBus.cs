using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects
{
    public class BattleBus : ScriptableObject
    {
        public BattleManager BattleManager { get; set; }
        public BattleInfo CurrentBattleInfo { get; set; }
        public Sprite CurrentBattleBackground { get; set; }
    }
}