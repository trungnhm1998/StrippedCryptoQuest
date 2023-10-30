using CryptoQuest.Character.Hero;
using IndiGames.Core.SaveSystem;
using System.Collections;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyProvider
    {
        HeroSpec[] GetParty();
        void SetParty(HeroSpec[] newSpecs);

        string ToJson();
        IEnumerator CoFromJson(string json);
    }
}