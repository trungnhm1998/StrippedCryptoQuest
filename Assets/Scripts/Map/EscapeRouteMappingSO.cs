using System;
using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Map
{
    [CreateAssetMenu(fileName = "Escape Route Mapping", menuName = "Crypto Quest/Map/Escape Route Mapping")]
    public class EscapeRouteMappingSO : ScriptableObject
    {
        public List<EscapeRouteMapping> EscapeRouteMappings;
    }

    [Serializable]
    public class EscapeRouteMapping
    {
        public List<SceneScriptableObject> EscapableMaps;
        public MapPathSO EscapeRoute;
    }
}