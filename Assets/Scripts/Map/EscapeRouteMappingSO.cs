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
        private Dictionary<SceneScriptableObject, MapPathSO> _mapToEscapePathDictionary = new();
        public Dictionary<SceneScriptableObject, MapPathSO> MapToEscapePathDictionary => _mapToEscapePathDictionary;

        private void OnEnable()
        {
            foreach (var escapeRouteMapping in EscapeRouteMappings)
            {
                foreach (var escapableMap in escapeRouteMapping.EscapableMaps)
                {
                    _mapToEscapePathDictionary[escapableMap] = escapeRouteMapping.EscapeRoute;
                }
            }
        }
    }

    [Serializable]
    public class EscapeRouteMapping
    {
        public List<SceneScriptableObject> EscapableMaps;
        public MapPathSO EscapeRoute;
    }
}