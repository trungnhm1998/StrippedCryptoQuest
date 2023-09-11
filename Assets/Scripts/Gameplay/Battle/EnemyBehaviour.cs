using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private EnemyData _enemyData;
        private GameObject _enemyModel;

        public void Init(Character.Enemy enemySpec)
        {
            _enemyData = enemySpec.Data;
            _enemyModel = Instantiate(_enemyData.Prefab, transform);
        }
    }
}