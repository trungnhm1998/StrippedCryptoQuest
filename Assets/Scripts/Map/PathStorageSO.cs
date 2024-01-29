using UnityEngine;
using System;
namespace CryptoQuest.Map
{
    [CreateAssetMenu(menuName = "Crypto Quest/Map/Map Transition")]
    public class PathStorageSO : ScriptableObject
    {
        [SerializeField] private MapPathSO _lastTakenPath;

        public event Action<MapPathSO> LastTakenPathChanged;

        public MapPathSO LastTakenPath
        {
            get => _lastTakenPath;
            set
            {
                _lastTakenPath = value;
                LastTakenPathChanged?.Invoke(_lastTakenPath);
            }
        }
    }
}

