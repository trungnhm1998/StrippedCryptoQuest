using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class EnemiesPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private UIEnemy _enemyPrefab;

        private readonly List<UIEnemy> _enemies = new();

        public void Init(List<EnemyBehaviour> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Spec.IsValid() == false) continue; // Model might not loaded yet
                var uiEnemy = Instantiate(_enemyPrefab, _content);
                uiEnemy.gameObject.SetActive(false);
                uiEnemy.Show(enemy);
                _enemies.Add(uiEnemy);
            }
        }

        public void Show()
        {
            var enemies = _enemyPartyManager.Enemies;
            bool selected = false;
            for (var index = 0; index < enemies.Count; index++)
            {
                if (!enemies[index].IsValid()) continue;

                enemies[index].SetAlpha(0.5f);

                var uiEnemy = _enemies[index];
                uiEnemy.Show(enemies[index]);
                uiEnemy.gameObject.SetActive(true);

                if (selected) continue;
                selected = true;
                uiEnemy.GetComponent<Button>().Select();
            }
        }

        public void Hide()
        {
            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                if (enemy == null) continue;
                enemy.gameObject.SetActive(false);
                _enemyPartyManager.Enemies[index].SetAlpha(1f);
            }
        }
    }
}