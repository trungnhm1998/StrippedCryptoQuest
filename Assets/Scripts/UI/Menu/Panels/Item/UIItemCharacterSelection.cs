using System;
using System.Collections.Generic;
using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIItemCharacterSelection : MonoBehaviour
    {
        [SerializeField] private List<MultiInputButton> _characterButtons;

        public void Init()
        {
            EnableAllButtons();
            _characterButtons[0].Select();
        }

        public void DeInit()
        {
            DisableAllButtons();
        }

        /// <summary>
        /// Add buttons to unity event system
        /// </summary>
        private void EnableAllButtons()
        {
            foreach (var button in _characterButtons)
            {
                button.enabled = true;
            }
        }

        /// <summary>
        /// Remove buttons from unity event system
        /// </summary>
        private void DisableAllButtons()
        {
            foreach (var button in _characterButtons)
            {
                button.enabled = false;
            }
        }
    }
}