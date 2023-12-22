using CryptoQuest.Character.Behaviours;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using CharacterBehaviour = CryptoQuest.Character.MonoBehaviours.CharacterBehaviour;

namespace CryptoQuest.Map
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] protected InputMediatorSO _inputMediator;
        [SerializeField] protected GameplayBus _gameplayBus;
        [SerializeField] protected GameObject _heroPrefab;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private CharacterBehaviour.EFacingDirection _defaultFacingDirection;

        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        private GoFrom[] _mapEntrances;
        private Transform _defaultSpawnPoint;

        private void Awake()
        {
            _mapEntrances = FindObjectsOfType<GoFrom>();
            _defaultSpawnPoint = transform.GetChild(0);
            OnAwake();
        }

        private Transform GetSpawnPoint()
        {
            var spawnPoint = _defaultSpawnPoint;

            foreach (var entrance in _mapEntrances)
            {
                if (entrance.MapPath != _pathStorage.LastTakenPath) continue;
                spawnPoint = entrance.transform;
                break;
            }

            return spawnPoint;
        }

        private void OnEnable()
        {
            _sceneLoadedEventChannelSO.EventRaised += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            _sceneLoadedEventChannelSO.EventRaised -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded()
        {
            SpawnPlayer();
            _inputMediator.EnableMapGameplayInput();
        }

        protected virtual void SpawnPlayer()
        {
            if (_defaultSpawnPoint == null) return;

            var spawnPoint = GetSpawnPoint();

            var heroInstance = Instantiate(_heroPrefab, spawnPoint.position, Quaternion.identity);
            if (heroInstance.TryGetComponent(out FacingBehaviour facingBehaviourComponent))
            {
                CharacterBehaviour.EFacingDirection facingDirection = _defaultFacingDirection;
                if (spawnPoint.TryGetComponent(out GoFrom goFromComponent))
                {
                    facingDirection = goFromComponent.EntranceFacingDirection;
                }
                facingBehaviourComponent.SetFacingDirection(facingDirection);
            }

            _gameplayBus.Hero = heroInstance.GetComponent<HeroBehaviour>();
            _gameplayBus.RaiseHeroSpawnedEvent();
        }

        protected virtual void OnAwake() { }
    }
}