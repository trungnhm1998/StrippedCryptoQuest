using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class EquipmentModelData : IData
    {
        private AssetReferenceT<Sprite> _icon;
        private LocalizedString _name;
        private bool _isEquipped;

        public EquipmentModelData(AssetReferenceT<Sprite> icon, LocalizedString name, bool isEquipped)
        {
            _icon = icon;
            _name = name;
            _isEquipped = isEquipped;
        }

        public EquipmentModelData(EquipmentInfo equipmentInfo)
        {
            _name = equipmentInfo.Data.DisplayName;
            _isEquipped = false;
        }

        public AssetReferenceT<Sprite> GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
        public bool IsEquipped() { return _isEquipped; }
    }
}
