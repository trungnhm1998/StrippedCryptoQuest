using System;
using CryptoQuest.Gameplay.Battle.Core.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    [CreateAssetMenu(menuName = "Gameplay/Party SO")]
    public class PartySO : ScriptableObject, IPartySort
    {
        public AbilitySystemBehaviour MainSystem;
        public BattleTeam PlayerTeam;
        public IPartySort PartySorter { get; private set; }
        public Action<bool> SortCompleted { get; set; }

        public void Sort(int sourceIndex, int destinationIndex)
        {
            PartySorter ??= new SimplePartySort(this);
            PartySorter.SortCompleted = SortCompleted;
            PartySorter.Sort(sourceIndex, destinationIndex);
        }
    }
}