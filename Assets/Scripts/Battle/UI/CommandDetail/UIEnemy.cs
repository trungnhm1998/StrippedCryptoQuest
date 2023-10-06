using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    internal class UIEnemy : MonoBehaviour
    {
        public event Action<EnemyBehaviour> Selected;
        [SerializeField] private TMP_Text _name;
        private EnemyBehaviour _enemy;

        private MultiInputButton _button;

        private void Awake()
        {
            _button = GetComponent<MultiInputButton>();
        }

        private void OnEnable()
        {
            _button.Selected += SelectEnemy;
            _button.DeSelected += DeSelectEnemy;
        }

        private void OnDisable()
        {
            _button.Selected -= SelectEnemy;
            _button.DeSelected -= DeSelectEnemy;
        }

        private void SelectEnemy()
        {
            SetAlphaEnemy(1);
        }

        private void DeSelectEnemy()
        {
            SetAlphaEnemy(0.5f);
        }

        public void SetAlphaEnemy(float alpha)
        {
            if (!_enemy.IsValid()) return;
            _enemy.SetAlpha(alpha);
        }

        public void Show(EnemyBehaviour enemy)
        {
            _enemy = enemy;
            _name.text = enemy.DisplayName;
        }

        public void Hide() { }

        public void OnPressed()
        {
            Selected?.Invoke(_enemy);
        }
    }
}