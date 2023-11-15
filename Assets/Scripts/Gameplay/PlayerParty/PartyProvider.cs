using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class PartyProvider : MonoBehaviour, IPartyProvider
    {
        [SerializeField] private PartySO _partySO;

        public PartySlotSpec[] GetParty() => _partySO.GetParty();
        public void SetParty(PartySlotSpec[] newSpecs) => _partySO.SetParty(newSpecs);
    }
}