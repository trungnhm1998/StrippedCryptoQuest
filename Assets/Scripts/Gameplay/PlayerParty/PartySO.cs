using System;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public static class PartyConstants
    {
        public const int PARTY_SIZE = 4;
    }

    public interface IParty
    {
        CharacterSpec[] Members { get; }
    }

    [CreateAssetMenu(menuName = "Gameplay/Party SO")]
    public class PartySO : ScriptableObject, IPartySort, IParty
    {
        [FormerlySerializedAs("Members")] [SerializeField] private CharacterSpec[] _members = new CharacterSpec[PartyConstants.PARTY_SIZE];

        public CharacterSpec[] Members => _members;

        [Header("Obsolete")]
        public AbilitySystemBehaviour MainSystem;

        public BattleTeam PlayerTeam;
        public IPartySort PartySorter { get; private set; }
        public Action<bool> SortCompleted { get; set; }

        private void OnValidate()
        {
            if (_members.Length != PartyConstants.PARTY_SIZE)
            {
                Array.Resize(ref _members, PartyConstants.PARTY_SIZE);
            }
        }

        public void Sort(int sourceIndex, int destinationIndex)
        {
            PartySorter ??= new SimplePartySort(this);
            PartySorter.SortCompleted = SortCompleted;
            PartySorter.Sort(sourceIndex, destinationIndex);
        }
    }
}