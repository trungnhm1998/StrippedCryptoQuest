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

        public void RenderData(EquipmentInfo equipment)
        {
            // _icon.sprite = equipment.Config.EquipmentType.Icon;
            // _displayName.StringReference = equipment.Config.DisplayName;
            _rarity.sprite = equipment.Rarity.Icon;
            _level.text = equipment.Level.ToString();
            LoadStar(equipment.Data.Stars);
            SetAttributes(equipment);
            // StartCoroutine(equipment.Config.Image.LoadSpriteAndSet(_illustration));
        }

        private void SetAttributes(EquipmentInfo equipment)
        {
            foreach (var attribute in _attributes)
            {
                SetStat(attribute, equipment);
            }
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
    }
}
