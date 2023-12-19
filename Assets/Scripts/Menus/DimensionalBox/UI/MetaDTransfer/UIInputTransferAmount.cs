using UnityEngine;
using System;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIInputTransferAmount : MonoBehaviour
    {
        public event Action<string> ValueChanged;

        [field: SerializeField] public Text _inputFieldText { get; private set; }
        [field: SerializeField] public InputField _inputField { get; private set; }
        [SerializeField] private Color _invalidColor;

        private Color _validColor;

        // This should be configurated in UI, but Unity still accept char in OnValueChanged
        public float InputedValue
        {
            get
            {
                if (float.TryParse(_inputField.text, out var value))
                    return value;
                return 0;
            }
        }
        
        public bool Interactable 
        {
            get => _inputField.interactable;
            set 
            {
                _inputField.interactable = value;
            }
        }

        private void Awake()
        {
            _validColor = _inputFieldText.color;
            _inputField.onValueChanged.AddListener((value) => OnValueChanged(value));
        }

        private void OnValueChanged(string value)
        {
            // Somehow it still receive character here
            if (!float.TryParse(value, out var _)) return;
            ValueChanged?.Invoke(value);
        }

        public void SetInputValid(bool isValid)
        {
            _inputFieldText.color = isValid ? _validColor : _invalidColor;
        }

        public void Select()
        {
            Interactable = true;
            _inputField.Select();
        }

        public void DeSelect()
        {
            Interactable = false;
            _inputField.text = string.Empty;
            SetInputValid(false);
        }
    }
}