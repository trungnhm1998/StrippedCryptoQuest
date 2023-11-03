using System.Collections;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyProvider
    {
        PartySlotSpec[] GetParty();
        void SetParty(PartySlotSpec[] newSpecs);
    }
}