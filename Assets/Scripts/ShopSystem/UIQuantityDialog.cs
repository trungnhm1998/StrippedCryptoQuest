using System;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.ShopSystem
{
    public class UIQuantityDialog : AbstractDialog
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private float _waitThresholds = 0.3f;

        [SerializeField] private float _increaseStepThresholds = 2f;
        [SerializeField] private int _jumpStep = 10;

        private void OnEnable()
        {
            _input.MenuNavigationContextEvent += ChangeQuantity;
        }

        private void OnDisable()
        {
            _quantityChanged = null;
            _input.MenuNavigationContextEvent -= ChangeQuantity;
        }

        private int _yAxis;

        private void ChangeQuantity(InputAction.CallbackContext ctx)
        {
            if (_maxQuantity == 0) return;
            _yAxis = (int)ctx.ReadValue<Vector2>().y;
            if (_yAxis == 0) return;
            CurrentQuantity += _yAxis;
        }

        private float _stepInterval;

        private void Update()
        {
            if (_yAxis == 0)
            {
                _multiplier = 0;
                _stepInterval = _increaseStepInterval = 0;
                return;
            }

            UpdateMultiplier();

            if (_stepInterval <= _waitThresholds)
            {
                _stepInterval += Time.deltaTime;
                return;
            }

            _stepInterval = 0;
            CurrentQuantity += _multiplier != 0 ? _multiplier : _yAxis;
        }

        private float _increaseStepInterval;
        private int _multiplier;
        private void UpdateMultiplier()
        {
            if (_increaseStepInterval <= _increaseStepThresholds)
            {
                _increaseStepInterval += Time.deltaTime;
                return;
            }

            _increaseStepInterval = 0;
            _multiplier += _jumpStep;
        }

        private int _maxQuantity;
        public int MaxQuantity => _maxQuantity;
        private int _currentQuantity;

        public int CurrentQuantity
        {
            get => _currentQuantity;
            private set
            {
                _currentQuantity = value;
                if (_currentQuantity <= 0) _currentQuantity = _maxQuantity;
                if (_currentQuantity > _maxQuantity) _currentQuantity = 1;
                _quantityChanged?.Invoke(_currentQuantity);
                _quantityText.text = _currentQuantity.ToString();
            }
        }

        public void Show(int maxQuantity)
        {
            _input.EnableMenuInput();
            _maxQuantity = maxQuantity;
            CurrentQuantity = 1;

            Show();
        }

        public override void Hide()
        {
            _quantityChanged = null;
            _input.DisableAllInput();
            base.Hide();
        }

        private Action<int> _quantityChanged;

        public UIQuantityDialog WithQuantityChangedCallback(Action<int> action)
        {
            _quantityChanged = action;
            return this;
        }
    }
}