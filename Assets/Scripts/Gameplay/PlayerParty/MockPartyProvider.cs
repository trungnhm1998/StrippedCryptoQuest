using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class MockPartyProvider : MonoBehaviour, IPartyProvider
    {
        [SerializeField] private MockParty _mockParty;

        public HeroSpec[] GetParty() => _mockParty.GetParty();
        public void SetParty(HeroSpec[] newSpecs) => _mockParty.SetParty(newSpecs);
    }
}