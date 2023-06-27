using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Map
{
    public class MapEntrance : MonoBehaviour
    {
        [SerializeField] private PathStorageSO _transitionSO;
        [SerializeField] private MapPathSO _mapPath;
        public MapPathSO MapPath => _mapPath;

        [SerializeField] private CharacterBehaviour.EFacingDirection _entranceFacingDirection;

        private void Start() { }
    }
}