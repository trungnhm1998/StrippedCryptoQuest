using CryptoQuest.AbilitySystem.Attributes;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// How hero auto target enemy
    /// </summary>
    public class HeroTargetBehaviour : MonoBehaviour, ITargeting
    {
        [SerializeField] private BattleBus _bus;
        private Character _target;

        /// <summary>
        /// hero this will be selected target if it's dead select next lowest hp target
        /// </summary>
        public Character Target
        {
            get
            {
                if (_target == null || _target.IsValidAndAlive()) return _target;
                var enemy = _bus.CurrentBattleContext.Enemies[0];
                enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);

                for (int i = 1; i < _bus.CurrentBattleContext.Enemies.Count; i++)
                {
                    var currentEnemy = _bus.CurrentBattleContext.Enemies[i];
                    if (currentEnemy.IsValidAndAlive() == false) continue;
                    // probably dead already
                    if (enemy.IsValidAndAlive() == false)
                    {
                        enemy = currentEnemy;
                        continue;
                    }

                    currentEnemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var currentHp);

                    if (currentHp.CurrentValue < hp.CurrentValue) enemy = currentEnemy;
                }

                _target = enemy;
                return _target;
            }
            set => _target = value;
        }
    }
}