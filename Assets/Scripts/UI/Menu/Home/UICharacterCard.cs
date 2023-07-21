using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Home
{
    public class UICharacterCard : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _beingSelected;
        [SerializeField] private GameObject _selectedEffect;

        private void OnEnable()
        {
            // _beingSelected.EventRaised += OnBeingSelected;
        }

        private void OnDisable()
        {
            // _beingSelected.EventRaised -= OnBeingSelected;
        }

        public void OnBeingSelected()
        {
            _selectedEffect.SetActive(true);
        }
    }
}
