using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class MockPartyProvider : MonoBehaviour, IPartyProvider
    {
        [FormerlySerializedAs("_mockParty")] [SerializeField] private PartySO _partySO;

        public PartySlotSpec[] GetParty() => _partySO.GetParty();
        public void SetParty(PartySlotSpec[] newSpecs) => _partySO.SetParty(newSpecs);
    }
}