using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public static class PartyConstants
    {
        public const int MAX_PARTY_SIZE = 4;
    }

    public interface IParty
    {
        CharacterSpec[] Members { get; }
        bool Sort(int sourceIndex, int destinationIndex);
    }

    [CreateAssetMenu(menuName = "Gameplay/Party SO")]
    public class PartySO : ScriptableObject, IParty
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

        /// <summary>
        /// Cannot sort into empty slot
        /// Both slot must be valid
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        public bool Sort(int sourceIndex, int destinationIndex)
        {
            if (sourceIndex is < 0 or >= PartyConstants.MAX_PARTY_SIZE)
            {
                Debug.LogError("PartySO::Sort::Invalid source index");
                return false;
            }

            if (destinationIndex is < 0 or >= PartyConstants.MAX_PARTY_SIZE)
            {
                Debug.LogError("PartySO::Sort::Invalid destination index");
                return false;
            }

            if (sourceIndex == destinationIndex)
            {
                Debug.LogWarning("PartyS::Sort::Source is the same as destination index");
                return false;
            }

            var destMember = Members[destinationIndex];
            if (destMember == null || destMember.IsValid() == false)
            {
                Debug.LogError("PartySO::Sort::Cannot sort into empty slot");
                return false;
            }

            var memberToSort = Members[sourceIndex];
            if (memberToSort == null || memberToSort.IsValid() == false)
            {
                Debug.LogError("PartySO::Sort::Invalid source or destination index");
                return false;
            }

            // Either this destructuring or 3 lines of code
            (Members[sourceIndex], Members[destinationIndex]) = (Members[destinationIndex], Members[sourceIndex]);

            Debug.Log($"Sorted {sourceIndex} to {destinationIndex}");
            return true;
        }
    }
}