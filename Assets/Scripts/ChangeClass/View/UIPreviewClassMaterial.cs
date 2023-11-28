using System.Collections.Generic;
using AssetReferenceSprite;
using CryptoQuest.Character.Hero;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIPreviewClassMaterial : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Image _element;
        [SerializeField] private Image _avatar;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private UIAttributeBar _expBar;
        [SerializeField] private List<UIAttribute> _attributeBar;

        public void PreviewCharacter(List<AttributeValue> value, UICharacter character)
        {
            _name.StringReference = character.Class.Origin.DetailInformation.LocalizedName;
            _element.sprite = character.Class.Elemental.Icon;
            _expBar.SetValue(character.CurrentExp);
            _expBar.SetMaxValue(character.RequireExp);
            _level.text = $"Lv{character.Level}";
            LoadAssetReference(character.Avatar);
            UpdateCharacterStats(value);
        }
        
        private void LoadAssetReference(AssetReferenceT<Sprite> avatar)
        {
            if (avatar == null)
            {
                _avatar.enabled = false;
                return;
            }
            StartCoroutine(avatar.LoadSpriteAndSet(_avatar));
        }

        private void UpdateCharacterStats(List<AttributeValue> attributeValues)
        {
            foreach (var attribute in attributeValues)
            {
                foreach (var attributeValue in _attributeBar)
                {
                    if (attribute.Attribute == attributeValue.Attribute)
                    {
                        attributeValue.SetValue(attribute.BaseValue);
                    }
                }
            }
        }
    }
}
