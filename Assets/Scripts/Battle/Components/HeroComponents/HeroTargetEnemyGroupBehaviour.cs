using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components.HeroComponents
{
    /// <summary>
    /// How hero auto target enemy group
    /// </summary>
    public class HeroTargetEnemyGroupBehaviour : MonoBehaviour
    {
        [SerializeField] private BattleBus _bus;
        private EnemyGroup _enemyGroup;

        /// <summary>
        /// Select next alive enemy group if current group is dead
        /// </summary>
        public EnemyGroup EnemyGroup
        {
            get
            {
                if (!_enemyGroup.IsValid()) return _enemyGroup;
            
                var enemiesAlive = _enemyGroup.GetAliveEnemies();
                if (enemiesAlive.Count > 0) return _enemyGroup;

                foreach (var group in _bus.CurrentBattleContext.EnemyGroups)
                {
                    var enemiesAliveInGroup = group.GetAliveEnemies();
                    if (enemiesAliveInGroup.Count > 0) return group;
                }

                return _enemyGroup;
            }
            set => _enemyGroup = value;
        }
    }
}