using IndiGames.GameplayAbilitySystem.AttributeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Stats
{
    public interface IAttributeUI
    {
        public void SetValue(float value);
    }

    public interface IAttributeComparable
    {
        public void CompareValue(float newValue);
    }

    public class UIAttribute : MonoBehaviour, IAttributeUI, IAttributeComparable
    {
        [SerializeField] private TMP_Text _valueLabel;
        [SerializeField] private Image _lowerIcon;
        [SerializeField] private Image _higherIcon;

        private int _convertedValue;

        public void CompareValue(float receivedValue)
        {
            switch (receivedValue)
            {
                case var _ when receivedValue > _convertedValue:
                    ResetAttributeUI();
                    UpdateAttributeUI(_higherIcon, receivedValue);
                    break;
                case var _ when receivedValue < _convertedValue:
                    ResetAttributeUI();
                    UpdateAttributeUI(_lowerIcon, receivedValue);
                    break;
                case var _ when receivedValue.NearlyEqual(_convertedValue):
                    ResetAttributeUI();
                    break;
            }
        }

        private void ResetAttributeUI()
        {
            _higherIcon.gameObject.SetActive(false);
            _lowerIcon.gameObject.SetActive(false);
            _valueLabel.color = Color.white;
        }

        private void UpdateAttributeUI(Image image, float value)
        {
            image.gameObject.SetActive(true);
            _valueLabel.color = image.color;
            _valueLabel.text = $"{(int)value}";
        }

        public void SetValue(float value)
        {
            _valueLabel.text = $"{(int)value}";
        }
    }
}