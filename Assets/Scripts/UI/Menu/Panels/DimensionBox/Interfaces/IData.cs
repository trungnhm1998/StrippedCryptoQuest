using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IData
    {
        public AssetReferenceT<Sprite> GetIcon();
        public LocalizedString GetLocalizedName();
        public bool IsEquipped();
    }
}