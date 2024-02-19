using System;
using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Menus.TownTransfer
{
    public class ConditionalTransferLocations : ScriptableObject
    {
        [field: SerializeField] public List<ConditionalTransferEntrance> Locations = new();
    }

    [Serializable]
    public class ConditionalTransferEntrance
    {
        public TownTransferPath Entrance;
        public SceneScriptableObject RequiredLocations;
    }
}