using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private HeroBehaviour _heroPrefab;
        [SerializeField] private PathStorageSO _pathStorage;

        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        private GoFrom[] _mapEntrances;

        private Transform _defaultSpawnPoint;

        private void Awake()
        {
            _mapEntrances = FindObjectsOfType<GoFrom>();
            _defaultSpawnPoint = transform.GetChild(0);
        }

        private Transform GetSpawnPoint()
        {
            var spawnPoint = _defaultSpawnPoint;

            for (int i = 0; i < _mapEntrances.Length; i++)
            {
                var entrance = _mapEntrances[i];
                if (entrance.MapPath == _pathStorage.LastTakenPath)
                {
                    spawnPoint = entrance.transform;
                    break;
                }
            }

            return spawnPoint;
        }

        private void OnEnable()
        {
            _sceneLoadedEventChannelSO.EventRaised += SpawnPlayer;
        }

        private void OnDisable()
        {
            _sceneLoadedEventChannelSO.EventRaised -= SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            if (_defaultSpawnPoint == null) return;

            var spawnPoint = GetSpawnPoint();

            var heroInstance = Instantiate(_heroPrefab, spawnPoint.position, Quaternion.identity);
            _gameplayBus.Hero = heroInstance;
        }
    }
}