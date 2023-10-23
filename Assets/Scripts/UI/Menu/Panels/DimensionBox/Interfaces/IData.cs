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
        public Sprite GetIcon();
        public LocalizedString GetLocalizedName();
    }

    public interface INFT : IData { }

    public interface IGame : IData
    {
        public bool IsEquipped();
    }

}