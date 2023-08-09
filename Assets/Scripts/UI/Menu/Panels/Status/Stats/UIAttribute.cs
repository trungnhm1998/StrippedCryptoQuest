using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Stats
{
    public class UIAttribute : MonoBehaviour
    {
        private const string FORMAT = "{0}";
        private const string ELEMENTAL_FORMAT = "+{0}%";

        public float DefaultValue = 100;
        public bool isElemental;

        [SerializeField] private TMP_Text _valueLabel;
        [SerializeField] private Image _lowerIcon;
        [SerializeField] private Image _higherIcon;
        
        [SerializeField] private AttributeScriptableObject _attributeType;
        public AttributeScriptableObject Attribute => _attributeType;
        
        private int _convertedValue;

        private void Start()
        {
            _convertedValue = (int)DefaultValue;
            ResetAttributeUI();
        }

        public void CompareValue(float receivedValue)
        {
            switch (receivedValue)
            {
                case var _ when (int)receivedValue > _convertedValue:
                    ResetAttributeUI();
                    UpdateAttributeUI(_higherIcon, (int)receivedValue);
                    break;
                case var _ when (int)receivedValue < _convertedValue:
                    ResetAttributeUI();
                    UpdateAttributeUI(_lowerIcon, (int)receivedValue);
                    break;
                case var _ when (int)receivedValue == _convertedValue:
                    ResetAttributeUI();
                    break;
            }
        }

        private void ResetAttributeUI()
        {
            _higherIcon.gameObject.SetActive(false);
            _lowerIcon.gameObject.SetActive(false);
            _valueLabel.color = Color.white;
            _valueLabel.text = string.Format(isElemental ? ELEMENTAL_FORMAT : FORMAT, (int)DefaultValue);
        }

        private void UpdateAttributeUI(Image image, int value)
        {
            image.gameObject.SetActive(true);
            _valueLabel.color = image.color;
            _valueLabel.text = string.Format(isElemental ? ELEMENTAL_FORMAT : FORMAT, value);
        }
    }
}