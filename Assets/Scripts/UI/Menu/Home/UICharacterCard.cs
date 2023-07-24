using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Home
{
    public class UICharacterCard : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedEffect;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        public void OnBeingSelected()
        {
            _selectedEffect.SetActive(true);
        }
    }
}
