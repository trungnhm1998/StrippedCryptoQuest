using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public static class PartyConstants
    {
        public const int PARTY_SIZE = 4;
    }

    public interface IPartyManager { }

    [Serializable]
    public class Party
    {
        public CharacterSpec[] Characters = new CharacterSpec[PartyConstants.PARTY_SIZE];
    }

    public class PartyManager : MonoBehaviour, IPartyManager
    {
        [SerializeField, Header("Party Config")]
        private PartySO _party;

        [SerializeField, Space] private PartySlot[] _partySlots = new PartySlot[PartyConstants.PARTY_SIZE];

        private void OnValidate()
        {
            if (_partySlots.Length != PartyConstants.PARTY_SIZE)
            {
                Array.Resize(ref _partySlots, PartyConstants.PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<PartySlot>();
        }

        private void Start()
        {
            InitParty();
        }

        private void InitParty()
        {
            for (int i = 0; i < _party.Members.Length; i++)
            {
                var character = _party.Members[i];
                if (character == null) continue;
                _partySlots[i].Init(character);
            }
        }
    }
}