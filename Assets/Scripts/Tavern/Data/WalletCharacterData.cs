using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.Data
{
    public class WalletCharacterData : IWalletCharacterData
    {
        private Sprite _classIcon;
        private LocalizedString _localizedName;
        private string _name;
        private int _level;

        public WalletCharacterData(Sprite classIcon, LocalizedString localizedName, int level)
        {
            _classIcon = classIcon;
            _localizedName = localizedName;
            _level = level;
        }

        public WalletCharacterData(Sprite classIcon, string name, int level)
        {
            _classIcon = classIcon;
            _name = name;
            _level = level;
        }

        public Sprite GetClassIcon() => _classIcon;
        public LocalizedString GetLocalizedName() => _localizedName;
        public string GetName() => _name;
        public int GetLevel() => _level;
    }
}