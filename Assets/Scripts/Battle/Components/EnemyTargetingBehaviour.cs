using CryptoQuest.Gameplay.Battle.Core.Helper;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// How enemy auto target hero
    /// </summary>
    internal class EnemyTargetingBehaviour : MonoBehaviour, ITargeting
    {
        [SerializeField] MonsterTargetPlayerWeightConfig _weightConfig;
        public Character Target { get; set; }

        public void UpdateTargetIfNeeded(BattleContext context)
        {
            var aliveHeroes = context.PlayerParty.OrderedAliveMembers;
            var weightRandomIndex =
                CommonBattleHelper.WeightedRandomIndex(_weightConfig.Weights[..aliveHeroes.Count]);
            var isIndexValid = (0 <= weightRandomIndex) && (weightRandomIndex < aliveHeroes.Count);
            var selectedUnit =
                isIndexValid ? aliveHeroes[weightRandomIndex] : aliveHeroes[0];
            Target=  selectedUnit.IsValidAndAlive() ? selectedUnit : null;
        }
    }
}