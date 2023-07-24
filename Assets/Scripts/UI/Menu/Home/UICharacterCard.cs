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

        public void Select()
        {
            _selectedEffect.SetActive(true);
        }

        public void Deselect()
        {
            _selectedEffect.SetActive(false);
        }
    }
}
