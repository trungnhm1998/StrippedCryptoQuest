using System;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.ShopSystem
{
    public class UIQuantityDialog : AbstractDialog
    {
        public event UnityAction<int> QuantityChanged;
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private TMP_Text _quantityText;

        private void OnEnable()
        {
            _input.MenuNavigateEvent += ChangeQuantity;
            _input.MenuConfirmedEvent += OnConfirmQuantity;
            _input.MenuCancelEvent += OnCancel;
        }

        private void OnDisable()
        {
            _input.MenuNavigateEvent -= ChangeQuantity;
            _input.MenuConfirmedEvent -= OnConfirmQuantity;
            _input.MenuCancelEvent -= OnCancel;
        }

        private void ChangeQuantity(Vector2 axis)
        {
            if (_maxQuantity == 0) return;
            if (axis.y == 0) return;
            CurrentQuantity += (int)axis.y;
        }

        public void OnConfirmQuantity()
        {
            _confirmCallback?.Invoke();
            _confirmCallback = null;
            Hide();
        }

        public void OnCancel()
        {
            _cancelCallback?.Invoke();
            _cancelCallback = null;
            Hide();
        }

        private int _maxQuantity;
        public int MaxQuantity => _maxQuantity;
        private int _currentQuantity;

        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                var newQuantity = Math.Clamp(value, 1, _maxQuantity);

                if (newQuantity == _currentQuantity) return;
                _currentQuantity = newQuantity;
                QuantityChanged?.Invoke(_currentQuantity);
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
            _input.DisableAllInput();
            _hideCallback?.Invoke();
            _hideCallback = null;
            base.Hide();
        }

        private Action _confirmCallback;

        public UIQuantityDialog WithConfirmCallback(Action confirmCallback)
        {
            _confirmCallback = confirmCallback;
            return this;
        }

        private Action _cancelCallback;

        public UIQuantityDialog WithCancelCallback(Action cancelCallback)
        {
            _cancelCallback = cancelCallback;
            return this;
        }

        private Action _hideCallback;

        public UIQuantityDialog WithHideCallback(Action action)
        {
            _hideCallback = action;
            return this;
        }
    }
}