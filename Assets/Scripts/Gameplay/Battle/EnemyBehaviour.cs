using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private EnemyDef _enemyDef;
        private GameObject _enemyModel;

        public void Init(Character.EnemySpec enemySpecSpec)
        {
            _enemyDef = enemySpecSpec.Data;
            _enemyModel = Instantiate(_enemyDef.Prefab, transform);
        }
    }
}