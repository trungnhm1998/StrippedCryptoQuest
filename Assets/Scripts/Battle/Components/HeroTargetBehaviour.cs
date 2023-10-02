using CryptoQuest.Character.Attributes;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// How hero auto target enemy
    /// </summary>
    public class HeroTargetBehaviour : MonoBehaviour, ITargeting
    {
        public Character Target { get; set; }

        /// <summary>
        /// Find new target if current target is dead
        /// </summary>
        public void UpdateTargetIfNeeded(BattleContext context)
        {
            if (Target == null || Target.IsValid()) return;
            var enemy = context.Enemies[0];
            enemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);

            for (int i = 1; i < context.Enemies.Count; i++)
            {
                var currentEnemy = context.Enemies[i];
                if (currentEnemy.IsValid() == false) continue;
                // probably dead already
                if (enemy.IsValid() == false)
                {
                    enemy = currentEnemy;
                    continue;
                }

                currentEnemy.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var currentHp);

                if (currentHp.CurrentValue < hp.CurrentValue) enemy = currentEnemy;
            }

            Target = enemy;
        }
    }
}