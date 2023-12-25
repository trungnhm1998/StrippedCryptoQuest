using UnityEngine;
using System;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIInputTransferValidator : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private int _limitDecimalPlaces = 4;

        private void Awake()
            => _inputField.onValueChanged.AddListener(ValidateValueChanged);

        public void ValidateValueChanged(string value)
        {
            var splits = value.Split('.');
            if (splits.Length < 2) return;
            var decimalPoints = splits[1];
            if (decimalPoints.Length > _limitDecimalPlaces)
            {
                _inputField.text = value[0..^1];
            }
        }
    }
}