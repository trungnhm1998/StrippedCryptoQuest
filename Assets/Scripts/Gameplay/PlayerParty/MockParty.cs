using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class MockParty : ScriptableObject, IPartyProvider
    {
        [SerializeField] private HeroSpec[] _heroSpecs;
        public HeroSpec[] GetParty() => _heroSpecs;
        public void SetParty(HeroSpec[] newSpecs) => _heroSpecs = newSpecs;
    }
}