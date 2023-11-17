using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Menu;
using CryptoQuest.ChangeClass.API;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.ChangeClass.View
{
    public class UICharacter : MonoBehaviour
    {
        public event Action<UICharacter> OnSubmit;
        public event Action<UICharacter> OnItemSelected;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private MultiInputButton _button;
        public CharacterAPI Class { get; private set; }
        public AssetReferenceT<Sprite> Avatar { get; private set; }
        public LocalizedString LocalizedName { get; private set; }
        public Sprite ElementImage { get; private set; }

        public void ConfigureCell(CharacterAPI characterClass)
        {
            Class = characterClass;
        }

        public void SyncData(LocalizedString localized, Sprite element, AssetReferenceT<Sprite> avatar)
        {
            Avatar = avatar;
            LocalizedName = localized;
            ElementImage = element;
            _displayName.StringReference = LocalizedName;
        }

        private void OnEnable()
        {
            _button.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
        }

        private void OnSelected()
        {
            OnItemSelected?.Invoke(this);
        }

        public void EnableButtonBackground(bool isEnable)
        {
            _selectedBackground.SetActive(isEnable);
        }
    }
}