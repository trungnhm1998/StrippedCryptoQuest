﻿using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconNFT;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private RectTransform _tooltipPosition;

        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;

        private EquipmentInfo _equipment = new();
        private ITooltip _tooltip;
        public EquipmentInfo Equipment => _equipment;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        }

        public void Init(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;
            _equipment = equipment;
            var def = equipment.Data;
            _nameText.text = equipment.Def.name;
            if (!def.DisplayName.IsEmpty) _nameLocalize.StringReference = def.DisplayName;
            _nameText.color = _enabledColor;
            _icon.sprite = def.EquipmentType.Icon;
            _iconNFT.SetActive(equipment.IsNftItem);
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        /// <summary>
        /// Open tooltip when inspecting
        /// Also called when deselecting a button but hide tooltip instead
        /// </summary>
        /// <param name="isInspecting"></param>
        public void OnInspecting(bool isInspecting)
        {
            if (_tooltip == null) return;

            if (isInspecting == false)
            {
                _tooltip.Hide();
                return;
            }

            if (_equipment.IsValid() == false) return;
            _tooltip
                .WithLevel(_equipment.Level)
                .WithDescription(_equipment.Data.DisplayName)
                .WithDisplaySprite(_equipment.Data.Image)
                .WithContentAwareness(_tooltipPosition)
                .WithRarity(_equipment.Rarity)
                .Show();
        }

        public void Reset()
        {
            _equipment = new EquipmentInfo();
        }
    }
}