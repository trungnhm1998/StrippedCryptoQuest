using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class ChildButtonsActivator : MonoBehaviour
    {
        [Header("Config Button Text Colors")]
        [SerializeField] private Color _activeColor;

        [SerializeField] private Color _inactiveColor;

        private readonly Dictionary<Button, TextMeshProUGUI[]> _buttonTextMap = new();
        private Button[] _allButtons;

        private void Awake()
        {
            _allButtons = GetComponentsInChildren<Button>();            
            CacheButtonTexts();
        }

        public void CacheButtonTexts()
        {
            if (_allButtons == null) return;
            _buttonTextMap.Clear();

            for (var i = 0; i < _allButtons.Length; i++)
            {
                var button = _allButtons[i];
                var buttonText = button.GetComponentsInChildren<TextMeshProUGUI>();
                _buttonTextMap.Add(_allButtons[i], buttonText);
            }
        }

        public void SetActiveButtons(bool isActive)
        {
            foreach (var pair in _buttonTextMap)
            {
                var button = pair.Key;
                var buttonTexts = pair.Value;

                button.interactable = isActive;
                foreach (var text in buttonTexts)
                {
                    text.color = isActive ? _activeColor : _inactiveColor;
                }
            }
        }
    }
}