using System;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    [CreateAssetMenu(menuName = "Gameplay/Party SO")]
    public class PartySO : ScriptableObject, IPartySort
    {
        public CharacterSpec[] Members = new CharacterSpec[PartyConstants.PARTY_SIZE];
        public AbilitySystemBehaviour MainSystem;
        public BattleTeam PlayerTeam;
        public IPartySort PartySorter { get; private set; }
        public Action<bool> SortCompleted { get; set; }

        private void OnValidate()
        {
            if (Members.Length != PartyConstants.PARTY_SIZE)
            {
                Array.Resize(ref Members, PartyConstants.PARTY_SIZE);
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