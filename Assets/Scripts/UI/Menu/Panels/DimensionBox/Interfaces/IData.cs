using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces
{
    public interface IData
    {
        public Sprite GetIcon();
        public LocalizedString GetLocalizedName();
    }
}