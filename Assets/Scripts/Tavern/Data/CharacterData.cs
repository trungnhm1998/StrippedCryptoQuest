using CryptoQuest.Battle.Components;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.Data
{
    public class CharacterData : ICharacterData
    {
        public int Id { get; set; }

        private Sprite _classIcon;
        private LocalizedString _localizedName;
        private string _name;
        private int _level;
        private bool _isInParty;

        public CharacterData(HeroBehaviour hero, bool isInParty)
        {
            hero.TryGetComponent(out LevelSystem levelSystem);
            _localizedName = hero.DetailsInfo.LocalizedName;
            _level = levelSystem.Level;
            _isInParty = isInParty;
        }

        public CharacterData(Sprite classIcon, LocalizedString localizedName, int level, bool isInParty)
        {
            _classIcon = classIcon;
            _localizedName = localizedName;
            _level = level;
            _isInParty = isInParty;
        }

        public CharacterData(string name, int level, bool isInParty)
        {
            _name = name;
            _level = level;
            _isInParty = isInParty;
        }

        public int GetId() => Id;
        public Sprite GetClassIcon() => _classIcon;
        public LocalizedString GetLocalizedName() => _localizedName;
        public string GetName() => _name;
        public int GetLevel() => _level;
        public bool IsInParty() => _isInParty;
    }
}