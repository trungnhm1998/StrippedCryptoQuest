using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.Interfaces
{
    public interface ICharacterData
    {
        public Sprite GetClassIcon();
        public LocalizedString GetLocalizedName();
        public string GetName();
        public int GetLevel();
    }

    public interface IWalletCharacterData : ICharacterData { }

    public interface IGameCharacterData : ICharacterData
    {
        public bool IsInParty();
    }
}