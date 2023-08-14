using System;
using System.Collections.Generic;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Ocarinas.Data
{
    [CreateAssetMenu(menuName = "Crypto Quest/Item/Ocarina Data SO")]
    public class OcarinaDefinition : ScriptableObject
    {
        [Serializable]
        public class Location
        {
            public LocalizedString MapName;
            public MapPathSO Path;
        }

        public List<Location> Locations;

        public List<SceneScriptableObject> OcarinaBlockSceneList;
    }
}