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
        [SerializeField] private TMP_Text _currentValue;
        [SerializeField] private TMP_Text _maxValue;
        [SerializeField] private Image _bar;

        private float _maxValueFloat = 0;

        public void SetMaxValue(float maxValue)
        {
            _maxValueFloat = maxValue;
            _bar.fillAmount = 1;
            _maxValue.text = $"{(int)maxValue}";
        }

        public void SetValue(float value)
        {
            _bar.fillAmount = 1;
            _currentValue.text = $"{(int)value}";
            _bar.fillAmount = value / _maxValueFloat;
        }
    }
}