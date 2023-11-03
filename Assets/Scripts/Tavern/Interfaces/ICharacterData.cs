using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.Interfaces
{
    public interface ICharacterData
    {
        public int GetId();
        public Sprite GetClassIcon();
        public LocalizedString GetLocalizedName();
        public string GetName();
        public int GetLevel();
        public bool IsInParty();
    }
}