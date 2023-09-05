using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private EnemyData _enemyData;
        private GameObject _enemyModel;

        public void Init(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _enemyModel = Instantiate(enemyData.Prefab, transform);
        }
    }
}