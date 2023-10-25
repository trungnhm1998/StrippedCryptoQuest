using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces
{
    public interface IData
    {
        public uint GetId();
        public Sprite GetIcon();
        public LocalizedString GetLocalizedName();
    }

    public interface INFT : IData { }

    public interface IGame : IData
    {
        public bool IsEquipped();
    }
}