using Core.Runtime.Events.ScriptableObjects;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private HeroBehaviour _heroPrefab;

        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        private Transform _defaultSpawnPoint;

        private void Awake()
        {
            _defaultSpawnPoint = transform.GetChild(0);
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

            var heroInstance = Instantiate(_heroPrefab, _defaultSpawnPoint);
            _gameplayBus.Hero = heroInstance;
        }
    }
}