using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class WalletEquipmentData : INFT
    {
        private Sprite _icon;
        private LocalizedString _name;

        public WalletEquipmentData(EquipmentInfo equipmentInfo)
        {
            _icon = equipmentInfo.Data.EquipmentType.Icon;
            _name = equipmentInfo.Data.DisplayName;
        }

        public Sprite GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
    }
}
