using UnityEngine.Localization;
using System;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartySort
    {
        Action<bool> SortCompleted { get; set; }
        void Sort(int sourceIndex, int destinationIndex);
    }
}