using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Map
{
    public class MapEntrance : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private MapPathSO _mapPath;
        public MapPathSO MapPath => _mapPath;
        [SerializeField] private CharacterBehaviour.EFacingDirection _entranceFacingDirection;
        public CharacterBehaviour.EFacingDirection EntranceFacingDirection => _entranceFacingDirection;

        [Header("Refs")]
        [SerializeField] private PathStorageSO _pathStorage;
    }
}