using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle
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
    