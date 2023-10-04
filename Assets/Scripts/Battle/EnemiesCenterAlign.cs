using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using UnityEngine;


namespace CryptoQuest.Battle
{
    public interface IGameObjectAlign<T>
    {
        void Align(IList<T> objects);
    }

    public class EnemiesCenterAlign : IGameObjectAlign<EnemyBehaviour>
    {
        private const float X_OFFSET = 2.63f;
        private const float Z_OFFSET = 0.001f;

        public void Align(IList<EnemyBehaviour> enemies)
        {   
            var validEnemies = enemies.Where(e => e.Spec.IsValid()).ToList();
            var centerPosition = Vector3.zero;
            float centerIndex = (validEnemies.Count - 1) / 2f;

            for (int i = 0; i < validEnemies.Count; i++)
            {
                var enemy = validEnemies[i];
                var enemyY = enemy.transform.position.y;
                enemy.transform.localPosition = centerPosition
                    + new Vector3(X_OFFSET * (i - centerIndex), enemyY, Z_OFFSET * i);
            }
        }
    }
}