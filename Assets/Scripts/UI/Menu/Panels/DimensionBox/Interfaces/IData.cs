using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IData
    {
        public string GetId();
        public Sprite GetIcon();
        public LocalizedString GetLocalizedName();
    }

    public interface INFT : IData { }

    public interface IGame : IData
    {
        public bool IsEquipped();
    }

}