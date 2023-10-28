using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking.Menu.DimensionBox;
using IndiGames.Core.Events.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using CharacterBehaviour = CryptoQuest.Character.MonoBehaviours.CharacterBehaviour;

namespace CryptoQuest.Map
{
    [Serializable]
    class SpawnData
    {
        public string LastTakenPathGuid;
    }

    public class SpawnManager : SaveObject
    {
        [SerializeField] protected InputMediatorSO _inputMediator;
        [SerializeField] protected GameplayBus _gameplayBus;
        [SerializeField] protected HeroBehaviour _heroPrefab;
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
                if (entrance.MapPath == _pathStorage.LastTakenPath)
                {
                    spawnPoint = entrance.transform;
                    break;
                }
            }

            return spawnPoint;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _sceneLoadedEventChannelSO.EventRaised += HandleSceneLoaded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _sceneLoadedEventChannelSO.EventRaised -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded()
        {
            StartCoroutine(CoSpawnPlayer());
            _inputMediator.EnableMapGameplayInput();
        }

        private IEnumerator CoSpawnPlayer()
        {
            yield return WaitUntilTrue(IsLoaded);
            SpawnPlayer();
        }

        protected virtual void SpawnPlayer()
        {
            if (_defaultSpawnPoint == null) return;

            var spawnPoint = GetSpawnPoint();

            var heroInstance = Instantiate(_heroPrefab, spawnPoint.position, Quaternion.identity);
            heroInstance.SetFacingDirection(spawnPoint.gameObject.TryGetComponent<GoFrom>(out var goFrom)
                ? goFrom.EntranceFacingDirection
                : _defaultFacingDirection);

            _gameplayBus.Hero = heroInstance;
            _gameplayBus.RaiseHeroSpawnedEvent();
            SaveSystem.SaveObject(this);
        }

        protected virtual void OnAwake() { }

        #region SaveSystem
        public override string Key => "SpawnManager";

        public override string ToJson()
        {
            if (_pathStorage != null && _pathStorage.LastTakenPath != null)
            {
                var spawnData = new SpawnData()
                {
                    LastTakenPathGuid = _pathStorage.LastTakenPath.Guid
                };
                return JsonUtility.ToJson(spawnData);
            }
            return null;
        }

        public override IEnumerator CoFromJson(string json)
        {
            if (_pathStorage != null && !string.IsNullOrEmpty(json))
            {
                var spawnData = new SpawnData();
                JsonUtility.FromJsonOverwrite(json, spawnData);
                if (!string.IsNullOrEmpty(spawnData.LastTakenPathGuid))
                {
                    var mapSoHandle = Addressables.LoadAssetAsync<MapPathSO>(spawnData.LastTakenPathGuid);
                    yield return mapSoHandle;
                    if (mapSoHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _pathStorage.LastTakenPath = mapSoHandle.Result;
                    }
                }
            }
        }
        #endregion
    }
}