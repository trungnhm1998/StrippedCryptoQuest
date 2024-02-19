using System;
using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Ocarina
{
    public class ConditionalOcarinaLocations : ScriptableObject
    {
        [field: SerializeField] public List<ConditionalOcarinaEntrance> Locations = new();
    }

    [Serializable]
    public class ConditionalOcarinaEntrance
    {
        public OcarinaEntrance Entrance;
        public SceneScriptableObject RequiredLocations;
    }
}