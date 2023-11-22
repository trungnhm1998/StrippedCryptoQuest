using System;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.Character.Hero;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UICharacter : MonoBehaviour
    {
        public event Action<UICharacter> OnSubmit;
        public event Action<UICharacter> OnItemSelected;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [field: SerializeField] public RectTransform Content { get; private set; }
        public HeroSpec Class { get; private set; }
        public AssetReferenceT<Sprite> Avatar { get; private set; }
        public Sprite ElementImage { get; private set; }
        public float CurrentExp { get; private set; }
        public float RequireExp { get; private set; }
        public int Level { get; private set; }

        public void ConfigureCell(HeroSpec characterClass)
        {
            Class = characterClass;
            _localizedName.StringReference = Class.Origin.DetailInformation.LocalizedName;
        }

        public void SyncAvatar(AssetReferenceT<Sprite> avatar)
        {
            Avatar = avatar;
        }

        public void CalculatorExp(float currentExp, float requireExp, int level)
        {
            RequireExp = requireExp;
            Level = level;
            CurrentExp = currentExp;
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