using System.Collections;
using System.Collections.Generic;
using AssetReferenceSprite;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
            _icon.sprite = equipment.Data.EquipmentType.Icon;
            _displayName.StringReference = equipment.Data.DisplayName;
            _rarity.sprite = equipment.Rarity.Icon;
            _level.text = equipment.Level.ToString();
            LoadStar(equipment.Def.Stars);
            SetAttributes(equipment);
            StartCoroutine(equipment.Data.Image.LoadSpriteAndSet(_illustration));
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
