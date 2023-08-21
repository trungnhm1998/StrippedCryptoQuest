using UnityEngine.Localization;
using System;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using CryptoQuest.Gameplay.PlayerParty.Helper;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class SimplePartySort : IPartySort
    {
        public Action<bool> SortCompleted { get; set; }
        
        private PartySO _party;
        
        public SimplePartySort(PartySO party)
        {
            _party = party;
        }

        public void Sort(int sourceIndex, int destinationIndex)
        {
            if (_party == null)
            {
                Debug.LogError("No party available");
                SortCompleted?.Invoke(false);
                return;
            }

            var members = _party.PlayerTeam.Members;

            if (!members.IsIndexValid(sourceIndex) || !members.IsIndexValid(destinationIndex))
            {
                SortCompleted?.Invoke(false);
                Debug.LogError("Invalid source or destination index");
                return;
            }

            if (sourceIndex == destinationIndex)
            {
                Debug.LogWarning("Source is the same as destination index");
                SortCompleted?.Invoke(true);
                return;
            }

            members.SortElement(sourceIndex, destinationIndex);

            Debug.Log($"Sorted {sourceIndex} to {destinationIndex}");
            SortCompleted?.Invoke(true);
        }
    }
}