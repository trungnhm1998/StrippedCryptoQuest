using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
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
        public CharacterSO[] Characters = new CharacterSO[PartyConstants.PARTY_SIZE];
    }

    public class PartyManager : MonoBehaviour, IPartyManager
    {
        [SerializeField, Header("Party Config")]
        private Party _party;

        [SerializeField, Space] private PartySlot[] _partySlots = new PartySlot[PartyConstants.PARTY_SIZE];

        private void OnValidate()
        {
            if (_party.Characters.Length != PartyConstants.PARTY_SIZE)
            {
                Array.Resize(ref _party.Characters, PartyConstants.PARTY_SIZE);
            }

            _partySlots = GetComponentsInChildren<PartySlot>();
        }

        private void Awake()
        {
            InitParty();
        }

        private void InitParty()
        {
            for (int i = 0; i < _party.Characters.Length; i++)
            {
                var character = _party.Characters[i];
                if (character == null) continue;

                IPartySlot slot = _partySlots[i];
                // slot.Init(character);
            }
        }
    }
}