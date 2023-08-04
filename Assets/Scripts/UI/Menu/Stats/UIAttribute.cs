using System;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Stats
{
    public class UIAttribute : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueLabel;
        [SerializeField] private Image _lowerIcon;
        [SerializeField] private Image _higherIcon;
        [SerializeField] private FloatEventChannelSO CompareValueEvent;

        private float _value = 100;
        private int _convertedValue;

        private void Start()
        {
            CompareValueEvent.EventRaised += CompareValue;
            _convertedValue = (int)_value;
        }

        private void CompareValue(float receivedValue)
        {
            switch (receivedValue)
            {
                case var _ when (int)receivedValue > _convertedValue:
                    ResetAttributeUI();
                    _higherIcon.gameObject.SetActive(true);
                    _valueLabel.color = _higherIcon.color;
                    break;
                case var _ when (int)receivedValue < _convertedValue:
                    ResetAttributeUI();
                    _lowerIcon.gameObject.SetActive(true);
                    _valueLabel.color = _lowerIcon.color;
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
        }
    }
}