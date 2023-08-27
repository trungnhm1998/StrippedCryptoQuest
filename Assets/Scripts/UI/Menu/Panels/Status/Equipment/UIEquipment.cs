using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private RectTransform _tooltipPosition;
        private EquipmentInfo _equipment = new();
        private ITooltip _tooltip;
        public EquipmentInfo Equipment => _equipment;

        private void Awake()
        {
            _tooltip = _tooltipProvider.Tooltip;
        }

        public void Init(EquipmentInfo equipment)
        {
            _equipment = equipment;
            var def = equipment.Data;
            _name.StringReference = def.DisplayName;
            _icon.sprite = def.EquipmentType.Icon;
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
                .WithDescription(_equipment.Data.DisplayName)
                .WithDisplaySprite(_equipment.Data.Image)
                .WithContentAwareness(_tooltipPosition)
                .Show();
        }

        public void Reset()
        {
            _equipment = new EquipmentInfo();
        }
    }
}