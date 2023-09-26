using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public static class PartyConstants
    {
        public const int MAX_PARTY_SIZE = 4;
    }

    public class PartySO : ScriptableObject
    {
        [SerializeField]
        private CharacterSpec[] _members = new CharacterSpec[PartyConstants.MAX_PARTY_SIZE];

        public CharacterSpec[] Members => _members;

        private void OnValidate()
        {
            if (_members.Length != PartyConstants.MAX_PARTY_SIZE)
            {
                Array.Resize(ref _members, PartyConstants.MAX_PARTY_SIZE);
            }
        }
    }
}