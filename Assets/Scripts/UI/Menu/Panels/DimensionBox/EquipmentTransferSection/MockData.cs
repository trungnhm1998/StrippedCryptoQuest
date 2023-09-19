using System;
using UnityEngine;
using UnityEngine.Localization;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    [Serializable]
    public struct MockData : IData
    {
        private Sprite _icon;
        private LocalizedString _name;

        public MockData(Sprite icon, LocalizedString name)
        {
            _icon = icon;
            _name = name;
        }

        public Sprite GetIcon() { return _icon; }
        public LocalizedString GetLocalizedName() { return _name; }
    }
}