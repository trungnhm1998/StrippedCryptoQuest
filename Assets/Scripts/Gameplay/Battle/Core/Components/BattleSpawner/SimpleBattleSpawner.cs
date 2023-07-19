using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner
{
    public class SimpleBattleSpawner : BaseBattleSpawner
    {
        [SerializeField] private BattleDataSO _battleData;
        public override void SpawnBattle()
        {
            GenerateBattle(_battleData);
        }
    }
}
    