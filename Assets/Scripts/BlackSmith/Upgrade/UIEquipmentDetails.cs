using System.Collections.Generic;
using AssetReferenceSprite;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIEquipmentDetails : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _rarity;
        [SerializeField] private Image _illustration;
        [SerializeField] private List<Image> _listStar;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Sprite _star;
        [SerializeField] private Sprite _defaultStar;
        [SerializeField] private List<UIAttribute> _attributes;
        private EquipmentInfo _equipment;

        public void RenderData(EquipmentInfo equipment)
        {
            _equipment = equipment;
            if (equipment == null || equipment.IsValid() == false) return;
            _icon.sprite = equipment.Type.Icon;
            _displayName.StringReference = equipment.Prefab.DisplayName;
            _rarity.sprite = equipment.Rarity.Icon;
            _level.text = equipment.Level.ToString();
            LoadStar(equipment.Data.Stars);
            SetAttributes(equipment);
            SetEquipmentImage(equipment);
        }

        private void SetAttributes(EquipmentInfo equipment)
        {
            foreach (var attribute in _attributes)
            {
                SetStat(attribute, equipment);
            }
        }

        private void SetEquipmentImage(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false) return;
            var isImageValid = equipment.Prefab.Image.RuntimeKeyIsValid();
            _illustration.enabled = isImageValid;
            if (isImageValid) StartCoroutine(equipment.Prefab.Image.LoadSpriteAndSet(_illustration));
        }

        private void SetStat(UIAttribute attribute, EquipmentInfo equipment)
        {
            foreach (var item in equipment.Stats)
            {
                attribute.SetValue(item.Attribute == attribute.Attribute ? item.Value : 0);
            }
        }

        private void LoadStar(int stars)
        {
            int currentStar = 0;
            foreach (var image in _listStar)
            {
                currentStar++;
                image.sprite = currentStar < stars ? _star : _defaultStar;
            }
        }

        private void OnDisable()
        {
            _equipment.Prefab.Image.ReleaseAsset();
        }
    }
}