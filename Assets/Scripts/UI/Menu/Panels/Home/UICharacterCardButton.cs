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

        [SerializeField] private Image _avatar;
        [SerializeField] private GameObject _selectedEffect;
        [SerializeField] private Image _selectedAvatar;
        [SerializeField] private GameObject _contents;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO SortModeEnabledEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            SortModeEnabledEvent.EventRaised += CheckButtonActive;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SortModeEnabledEvent.EventRaised -= CheckButtonActive;
        }

        private void CheckButtonActive()
        {
            // this.enabled = _contents.activeSelf;
        }

        public void CardButtonOnPressed()
        {
            PerformSelectedEffect();
            SelectedEvent?.Invoke(this);
        }

        public void Cancel()
        {
            BackToNormalState();
        }

        public void Confirm()
        {
            BackToNormalState();
        }

        private void PerformSelectedEffect()
        {
            _contents.SetActive(false);
            _selectedEffect.SetActive(true);
            _selectedAvatar.sprite = _avatar.sprite;
        }

        private void BackToNormalState()
        {
            _selectedEffect.SetActive(false);
            _contents.SetActive(true);
        }
    }
}