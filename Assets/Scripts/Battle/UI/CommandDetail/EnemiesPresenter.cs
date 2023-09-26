using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class EnemiesPresenter : MonoBehaviour
    {
        [SerializeField] private BattleManager _battleManager;
        [SerializeField] private Transform _content;
        [SerializeField] private UIEnemy _enemyPrefab;

        private readonly List<UIEnemy> _enemies = new();

        private void OnEnable()
        {
            _battleManager.Initialized += Init;
        }

        private void OnDisable()
        {
            _battleManager.Initialized -= Init;
        }

        private void Init()
        {
            var enemies = _battleManager.Enemies;
            foreach (var t in enemies)
            {
                if (t.IsValid() == false) continue;
                var uiEnemy = Instantiate(_enemyPrefab, _content);
                uiEnemy.gameObject.SetActive(false);
                uiEnemy.Show(t);
                _enemies.Add(uiEnemy);
            }
        }

        public void Show()
        {
            var enemies = _battleManager.Enemies;
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
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
}