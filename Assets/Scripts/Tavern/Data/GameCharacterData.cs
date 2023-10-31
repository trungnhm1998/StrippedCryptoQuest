using CryptoQuest.Battle.Components;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.Data
{
    public class GameCharacterData : IGameCharacterData
    {
        private Sprite _classIcon;
        private LocalizedString _localizedName;
        private string _name;
        private int _level;
        private bool _isInParty;

        public GameCharacterData(HeroBehaviour hero)
        {
            _localizedName = hero.DetailsInfo.LocalizedName;
            _level = hero.Level;
            _isInParty = true;
        }

        public GameCharacterData(LocalizedString localizedName, int level)
        {
            _localizedName = localizedName;
            _level = level;
            _isInParty = false;
        }

        public GameCharacterData(Sprite classIcon, string name, int level)
        {
            _classIcon = _classIcon;
            _name = name;
            _level = level;
            _isInParty = false;
        }

        public Sprite GetClassIcon() => _classIcon;
        public LocalizedString GetLocalizedName() => _localizedName;
        public string GetName() => _name;
        public int GetLevel() => _level;
        public bool IsInParty() => _isInParty;
    }
}