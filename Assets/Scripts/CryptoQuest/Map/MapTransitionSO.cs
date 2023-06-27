using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CryptoQuest.Map
{
    [CreateAssetMenu(menuName = "Crypto Quest/Map/Map Transition")]
    public class MapTransitionSO : ScriptableObject
    {
        public MapPathSO currentMapPath;
    }
}

