using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Character
{
    public interface IAttributeUI
    {
        void SetValue(float value);
    }

    public interface IAttributeComparable
    {
        void CompareValue(float oldValue, float newValue);
    }

    public class UIAttribute : MonoBehaviour, IAttributeUI, IAttributeComparable
    {
        [SerializeField] private AttributeScriptableObject _attribute;
        public AttributeScriptableObject Attribute => _attribute;

        [SerializeField] private TMP_Text _valueLabel;
        [SerializeField] private Image _lowerIcon;
        [SerializeField] private Image _higherIcon;

        private float _currentValue;
        private bool _isComparing = false;

        /// <summary>
        /// Reset Attribute UI and update UI if there's difference
        /// </summary>
        /// <param name="receivedValue">Value to compare</param>
        public void CompareValue(float oldValue, float receivedValue)
        {
            ResetAttributeUI();
            if (receivedValue.NearlyEqual(oldValue)) return;

            var icon = (receivedValue < oldValue) ? _lowerIcon : _higherIcon;
            PreviewAttributeUI(icon, receivedValue);
        }

        public void ResetAttributeUI()
        {
            _isComparing = false;
            _higherIcon.gameObject.SetActive(false);
            _lowerIcon.gameObject.SetActive(false);
            _valueLabel.color = Color.white;
            _valueLabel.text = $"{(int)_currentValue}";
        }

        /// <summary>
        /// Will be invoked when the attribute change
        /// Setup at event in scene 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            if (_isComparing) return;
            _currentValue = value;
            _valueLabel.text = $"{(int)value}";
        }

        private void PreviewAttributeUI(Image iconImage, float value)
        {
            _isComparing = true;
            iconImage.gameObject.SetActive(true);
            _valueLabel.color = iconImage.color;
            _valueLabel.text = $"{(int)value}";
        }
    }
}