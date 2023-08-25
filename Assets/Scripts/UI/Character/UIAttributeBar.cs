using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Character
{
    public interface IUIAttributeBar
    {
        void SetMaxValue(float maxValue);
        void SetValue(float value);
    }

    /// <summary>
    /// To use with HP, MP, Exps
    /// </summary>
    public class UIAttributeBar : MonoBehaviour, IUIAttributeBar
    {
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        [SerializeField] private TMP_Text _currentValue;
        [SerializeField] private TMP_Text _maxValue;
        [SerializeField] private Image _bar;

        private float _maxValueFloat;
        
        public void SetMaxValue(float maxValue)
        {
            _maxValueFloat = maxValue;
            _maxValue.text = $"{(int)maxValue}";
        }

        public void SetValue(float value)
        {
            _currentValue.text = $"{(int)value}";
            _bar.fillAmount = value / _maxValueFloat;
        }
    }
}