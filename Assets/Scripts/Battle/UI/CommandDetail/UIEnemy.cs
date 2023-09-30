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
            _button.Selected += SelectEnemy;
            _button.DeSelected += DeSelectEnemy;
        }

        private void OnDestroy()
        {
            _button.Selected -= SelectEnemy;
            _button.DeSelected -= DeSelectEnemy;
        }

        private void SelectEnemy()
        {
            _enemy.SetAlpha(1);
        }

        private void DeSelectEnemy()
        {
            _enemy.SetAlpha(0.5f);
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