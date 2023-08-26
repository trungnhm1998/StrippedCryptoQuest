using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private RectTransform _tooltipPosition;
        private EquipmentInfo _equipment = new();
        public ITooltip Tooltip { get; set; }

        public void Init(EquipmentInfo equipment)
        {
            _equipment = equipment;
            var def = equipment.Data;
            _name.StringReference = def.DisplayName;
            _icon.sprite = def.EquipmentType.Icon;
        }

        public void OnInspecting(bool isInspecting)
        {
            if (isInspecting == false)
            {
                Tooltip.Hide();
                return;
            }

            if (_equipment.IsValid() == false) return;
            Tooltip
                .WithDescription(_equipment.Data.DisplayName)
                .WithDisplaySprite(_equipment.Data.Image)
                .WithContentAwareness(_tooltipPosition)
                .Show();
        }
    }
}