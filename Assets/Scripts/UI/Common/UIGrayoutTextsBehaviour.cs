using System;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Common
{
    public class UIGrayoutTextsBehaviour : MonoBehaviour
    {
        [Header("Config Button Text Colors")]
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;

        [SerializeField] private TextMeshProUGUI[] _texts = Array.Empty<TextMeshProUGUI>();

        private void OnValidate()
        {
            _texts = GetComponentsInChildren<TextMeshProUGUI>(true);
        }

        public void SetGrayoutTexts(bool isActive)
        {
            foreach (var text in _texts)
            {
                text.color = isActive ? _activeColor : _inactiveColor;
            }
        }
    }
}