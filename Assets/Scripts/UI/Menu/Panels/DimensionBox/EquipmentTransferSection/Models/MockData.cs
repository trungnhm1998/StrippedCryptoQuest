using System;
using UnityEngine;
using UnityEngine.Localization;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    [Serializable]
    public struct MockData : IData
    {
        private Sprite _icon;
        private LocalizedString _name;
        private bool _isEquipped;

        public MockData(Sprite icon, LocalizedString name, bool isEquipped)
        {
            _icon = icon;
            _name = name;
            _isEquipped = isEquipped;
        }

        public Sprite GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
        public bool IsEquipped() { return _isEquipped; }
    }
}