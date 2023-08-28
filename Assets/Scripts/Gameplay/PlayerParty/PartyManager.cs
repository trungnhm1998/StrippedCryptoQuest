using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyController
    {
        bool TryGetMemberAtIndex(int charIndexInParty, out CharacterBehaviour character);
        IParty Party { get; }
    }

    public class PartyManager : MonoBehaviour, IPartyController
    {
        [field: SerializeField, Header("Party Config")]
        private PartySO _party;
        public IParty Party => _party;
        [SerializeField] private ServiceProvider _provider;

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
            _provider.Provide(this);
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

        public bool TryGetMemberAtIndex(int charIndexInParty, out CharacterBehaviour character)
        {
            character = _partySlots[0].Character; // first slot suppose to never be null

            if (charIndexInParty < 0 || charIndexInParty >= _partySlots.Length) return false;
            var partySlot = _partySlots[charIndexInParty];
            if (partySlot.IsValid() == false) return false;

            character = partySlot.Character;

            return true;
        }
    }
}