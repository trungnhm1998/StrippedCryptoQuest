using UnityEngine;
namespace CryptoQuest.Map
{
    [CreateAssetMenu(menuName = "Crypto Quest/Map/Map Transition")]
    public class PathStorageSO : ScriptableObject
    {
        public MapPathSO LastTakenPath;
    }
}

