using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Map/Map Storage")]
    public class MapStorageSO : ScriptableObject
    {
        public SceneScriptableObject currentMapScene;
    }
}