using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class PartyManager : MonoBehaviour
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

        private void Awake()
        {
            InitParty();
        }

        /// <summary>
        /// Init party members stats at run time
        /// and bind the mono behaviour to the <see cref="CharacterSpec._characterComponent"/>
        /// </summary>
        private void InitParty()
        {
            for (int i = 0; i < _party.Members.Length; i++)
            {
                var character = _party.Members[i];
                if (!character.IsValid()) continue;
                _partySlots[i].Init(character);
            }
        }
    }
}