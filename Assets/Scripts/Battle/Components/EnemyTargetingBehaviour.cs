using CryptoQuest.Gameplay.Battle.Core.Helper;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// How enemy auto target hero
    /// </summary>
    internal class EnemyTargetingBehaviour : MonoBehaviour, ITargeting
    {
        [SerializeField] private BattleBus _bus;
        [SerializeField] MonsterTargetPlayerWeightConfig _weightConfig;
        private Character _target;


        /// <summary>
        /// will be random target
        /// </summary>
        public Character Target
        {
            get
            {
                var aliveHeroes = _bus.CurrentBattleContext.PlayerParty.OrderedAliveMembers;
                var weightRandomIndex =
                    CommonBattleHelper.WeightedRandomIndex(_weightConfig.Weights[..aliveHeroes.Count]);
                var isIndexValid = (0 <= weightRandomIndex) && (weightRandomIndex < aliveHeroes.Count);
                var selectedUnit =
                    isIndexValid ? aliveHeroes[weightRandomIndex] : aliveHeroes[0];
                Target = selectedUnit.IsValidAndAlive() ? selectedUnit : null;
                return _target;
            }
            set => _target = value;
        }
    }
}