using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class GameEquipmentData : IGame
    {
        private Sprite _icon;
        private LocalizedString _name;
        private bool _isEquipped;

        public GameEquipmentData(EquipmentInfo equipmentInfo)
        {
            _icon = equipmentInfo.Data.EquipmentType.Icon;
            _name = equipmentInfo.Data.DisplayName;
            _isEquipped = equipmentInfo.IsEquipped;
        }

        public Sprite GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
        public bool IsEquipped() { return _isEquipped; }
    }
}
