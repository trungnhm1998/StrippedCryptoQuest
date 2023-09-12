using System.Collections.Generic;
using CryptoQuest.Menu;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UICharacterCardButton : MultiInputButton
    {
        public static UnityAction<UICharacterCardButton> SelectedEvent;

        [SerializeField] private GameObject _selectedEffect;

        public void CardButtonOnPressed()
        {
            SelectedEvent?.Invoke(this);
            _selectedEffect.SetActive(true);
        }

        public void BackToNormalState()
        {
            _selectedEffect.SetActive(false);
        }
    }
}