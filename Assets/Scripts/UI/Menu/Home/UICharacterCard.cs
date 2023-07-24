using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Home
{
    public class UICharacterCard : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private GameObject _selectingEffect;
        [SerializeField] private GameObject _selectedEffect;
        [SerializeField] private Image _selectedAvatar;

        private void OnEnable()
        {
            _selectedEffect.SetActive(false);
        }

        public void Select()
        {
            _selectingEffect.SetActive(true);
        }

        public void Deselect()
        {
            _selectingEffect.SetActive(false);
        }

        public void OnSelected()
        {
            PerformSelectedEffect();
        }

        private void PerformSelectedEffect()
        {
            _selectedEffect.SetActive(true);
            _selectedAvatar.sprite = _avatar.sprite;
        }
    }
}
