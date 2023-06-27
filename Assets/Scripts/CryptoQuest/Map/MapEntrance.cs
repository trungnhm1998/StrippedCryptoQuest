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
        [SerializeField] private CharacterArgsEventChannelSO _updatePlayerStateEvent;
        private void Start()
        {
            if (_transitionSO.LastTakenPath == null)
            {
                return;
            };
            if (_mapPath != _transitionSO.LastTakenPath) return;
            CharacterArgs characterArgs = new CharacterArgs();
            characterArgs.position = transform.position;
            characterArgs.facingDirection = _entranceFacingDirection;
            _updatePlayerStateEvent.RaiseEvent(characterArgs);
        }
    }
}