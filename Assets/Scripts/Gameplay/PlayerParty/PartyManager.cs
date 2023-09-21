using System;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyController
    {
        bool TryGetMemberAtIndex(int charIndexInParty, out CharacterBehaviourBase character);
        IParty Party { get; }
    }

    public class PartyManager : MonoBehaviour, IPartyController
    {
        [SerializeField] private AttributeSets _attributeSets; // Just for the asset to load
        [field: SerializeField, Header("Party Config")]
        private PartySO _party;
        public IParty Party => _party;
        [SerializeField] private ServiceProvider _provider;

        [SerializeField, Space] private PartySlot[] _partySlots = new PartySlot[PartyConstants.MAX_PARTY_SIZE];

        private void OnValidate()
        {
            if (_partySlots.Length != PartyConstants.MAX_PARTY_SIZE)
            {
                Array.Resize(ref _partySlots, PartyConstants.MAX_PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<PartySlot>();
        }

        // TODO: Remove below if refactor InitParty back to Awake 
        /// <summary>
        /// Note: If you using party at awake or start and active in the same scene with this
        /// You should get this through binding event <see cref="ServiceProvider.PartyProvided"/>
        /// </summary>
        private void Start()
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

        public bool TryGetMemberAtIndex(int charIndexInParty, out CharacterBehaviourBase character)
        {
            character = _partySlots[0].CharacterComponent; // first slot suppose to never be null

            if (charIndexInParty < 0 || charIndexInParty >= _partySlots.Length) return false;
            var partySlot = _partySlots[charIndexInParty];
            if (partySlot.IsValid() == false) return false;

            character = partySlot.CharacterComponent;

            return true;
        }
    }
}