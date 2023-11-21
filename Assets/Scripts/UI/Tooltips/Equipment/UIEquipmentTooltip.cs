﻿using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips.Equipment
{
    public interface ITooltipEquipmentProvider
    {
        public EquipmentInfo Equipment { get; }
    }

    public class UIEquipmentTooltip : UITooltipBase
    {
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Image _rarity;
        [SerializeField] private GameObject _nftTag;
        [SerializeField] private Image _illustration;
        [SerializeField] private TMP_Text _lvl;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private UIStars _uiStars;
        [SerializeField] private RectTransform _statsContainer;
        [SerializeField] private UIAttribute _attributeValuePrefab;

        private EquipmentInfo _equipment;

        private string _lvlText;
        private string LvlText => _lvlText ??= _lvl.text;

        protected override bool CanShow()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return false;
            var provider = selectedGameObject.GetComponent<ITooltipEquipmentProvider>();
            if (provider == null) return false;
            if (provider.Equipment == null || provider.Equipment.IsValid() == false) return false;
            _equipment = provider.Equipment;
            return true;
        }

        protected override void Init()
        {
            var behaviours = GetComponents<TooltipBehaviourBase>();
            foreach (var behaviour in behaviours) behaviour.Setup();
            SetupInfo();
            SetupStats();
            _illustration.LoadSpriteAndSet(_equipment.Config.Image);
        }

        private void SetupInfo()
        {
            _illustration.enabled = false;
            _headerBackground.color = _equipment.Rarity.Color;
            _rarity.sprite = _equipment.Rarity.Icon;
            _nftTag.SetActive(_equipment.IsNftItem);
            _nameLocalize.StringReference = _equipment.DisplayName;

            _lvl.text = string.Format(LvlText, _equipment.Level, _equipment.Data.MaxLevel);
            _uiStars.SetStars(_equipment.Data.Stars);
        }

        private void SetupStats()
        {
            foreach (Transform attribute in _statsContainer) Destroy(attribute.gameObject);
            foreach (var attribute in _equipment.Data.Stats)
            {
                if (_attributeConfigMapping.TryGetMap(attribute.Attribute, out var config) == false) continue;
                var attributeValue = Instantiate(_attributeValuePrefab, _statsContainer);
                var value = attribute.Value;
                value += (_equipment.Level - 1) * _equipment.Data.ValuePerLvl;
                attributeValue.SetAttribute(config.Name, value);
            }
        }

        private void OnDisable()
        {
            if (_equipment == null ||
                _equipment.Config == null ||
                _equipment.Config.Image.RuntimeKeyIsValid() == false)
                return;
            _equipment.Config.Image.ReleaseAsset();
            _equipment = null;
        }
    }
}