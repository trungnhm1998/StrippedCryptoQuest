﻿using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    public class SelectEnemyPresenter : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private UIEnemy _enemyPrefab;
        [SerializeField] private VerticalButtonSelector _buttonSelector;

        private readonly List<UIEnemy> _enemies = new();

        public void Init(List<EnemyBehaviour> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Spec.IsValid() == false) continue; // Model might not loaded yet
                var uiEnemy = Instantiate(_enemyPrefab, _content);
                uiEnemy.gameObject.SetActive(false);
                uiEnemy.Show(enemy);
                uiEnemy.Selected += OnEnemySelected;
                _enemies.Add(uiEnemy);
            }
        }

        private Action<EnemyBehaviour> _enemySelectedCallback;

        /// <summary>
        /// Using callback here to avoid remove/add listener every time we show/hide the UI
        /// </summary>
        public void RegisterEnemySelectedCallback(Action<EnemyBehaviour> callback)
        {
            _enemySelectedCallback = callback;
        }

        private void OnEnemySelected(EnemyBehaviour enemy)
        {
            _enemySelectedCallback?.Invoke(enemy);
        }

        public void Show()
        {
            var enemies = _enemyPartyManager.Enemies;
            bool selected = false;
            for (var index = 0; index < enemies.Count; index++)
            {
                if (!enemies[index].IsValidAndAlive()) continue;

                enemies[index].SetAlpha(0.5f);

                var uiEnemy = _enemies[index];
                uiEnemy.Show(enemies[index]);
                uiEnemy.transform.SetAsLastSibling();
                uiEnemy.gameObject.SetActive(true);

                if (selected) continue;
                enemies[index].SetAlpha(1);
                selected = true;
            }

            _buttonSelector.SelectFirstButton();
        }

        public void Hide()
        {
            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemyUI = _enemies[index];
                if (enemyUI == null) continue;
                enemyUI.SetAlphaEnemy(1f);
                enemyUI.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy) continue; // TODO: Why this happen?
                enemy.Selected -= OnEnemySelected;
                Destroy(enemy.gameObject);
            }

            _enemies.Clear();
        }
    }
}