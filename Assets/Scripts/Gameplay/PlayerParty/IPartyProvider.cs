using CryptoQuest.Character.Hero;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyProvider
    {
        HeroSpec[] GetParty();
        void SetParty(HeroSpec[] newSpecs);
    }
}