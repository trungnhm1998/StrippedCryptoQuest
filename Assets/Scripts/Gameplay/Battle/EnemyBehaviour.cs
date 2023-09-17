using CryptoQuest.Character.Enemy;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private string _displayName;
        private EnemySpec _spec = new();
        private EnemyDef _enemyDef;
        private GameObject _enemyModel;

        public void Init(EnemySpec enemySpec)
        {
            _spec = enemySpec;
            _spec.NameChanged += SetDisplayName;
            _displayName = _spec.DisplayName;
            _enemyDef = _spec.Data;
            _enemyModel = Instantiate(_enemyDef.Prefab, transform);
        }

        private void OnDestroy()
        {
            if (_spec.IsValid() == false) return; // party not always full
            _spec.NameChanged -= SetDisplayName;
            _spec.Release();
            Destroy(_enemyModel);
        }

        private void SetDisplayName(string value) => _displayName = value;
    }
}