using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class MockPartyProvider : MonoBehaviour, IPartyProvider
    {
        [SerializeField] private MockParty _mockParty;

        public PartySlotSpec[] GetParty() => _mockParty.GetParty();
        public void SetParty(PartySlotSpec[] newSpecs) => _mockParty.SetParty(newSpecs);
    }
}