using System.Collections;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Ship;
using IndiGames.Core.Events.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Gameplay.Ship
{
    [TestFixture, Category("Integration")]
    public class ShipManagerTests
    {
        private ShipBus _shipBus;
        private VoidEventChannelSO _spawnAllShipsEvent;
        private VoidEventChannelSO _sceneLoadedEvent;
        private GameplayBus _gameplayBus;
        private bool _sceneLoaded;

        private const string TEST_SCENE_PATH = "Assets/Scenes/WIP/TestShipMap.unity";

        [SetUp]
        public void Setup()
        {
            // LogAssert.ignoreFailingMessages = true;
            _shipBus = AssetDatabase.LoadAssetAtPath<ShipBus>(
                "Assets/ScriptableObjects/GameplayBuses/ShipBus.asset");
            _spawnAllShipsEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/Ship/SpawnAllShipsEventChannel.asset");
            _sceneLoadedEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/SceneManagement/SceneLoadedEventChannel.asset");
            _gameplayBus = AssetDatabase.LoadAssetAtPath<GameplayBus>(
                "Assets/ScriptableObjects/GameplayBuses/Gameplay.asset");

        }

        [UnityTest]
        public IEnumerator RequestSpawnShip_ShipNotActivated_NoShipSpawned()
        {
            _shipBus.IsShipActivated = false;
            yield return LoadScene(TEST_SCENE_PATH);

            Assert.AreEqual(0, GetShipInScene().Length);
        }

        [UnityTest]
        public IEnumerator RequestSpawnShip_ShipActivated_NotSailed_ShipEqualSpawnPoint()
        {
            _shipBus.IsShipActivated = true;
            _shipBus.HasSailed = false;
            yield return LoadScene(TEST_SCENE_PATH);

            var shipSpawnPoints = GameObject.FindObjectsByType<ShipSpawnPoint>(FindObjectsSortMode.None);
            Assert.AreEqual(shipSpawnPoints.Length, GetShipInScene().Length);
        }

        [UnityTest]
        public IEnumerator RequestSpawnShip_ShipActivatedAndSailed_ThereOnly1ShipAtPosition()
        {
            _shipBus.LastPosition = new SerializableVector2(new Vector2(100, 100));
            _shipBus.IsShipActivated = true;
            _shipBus.HasSailed = true;
            yield return LoadScene(TEST_SCENE_PATH);

            var ships = GetShipInScene();
            Assert.AreEqual(1, ships.Length);

            Assert.AreEqual(_shipBus.LastPosition.ToVector3(), ships[0].transform.position);
        }

        [UnityTest]
        public IEnumerator RespawnAllShips_AfterShipActivatedAndSailed_ShipsReseted()
        {
            _shipBus.LastPosition = new SerializableVector2(new Vector2(100, 100));
            _shipBus.IsShipActivated = true;
            _shipBus.HasSailed = true;
            yield return LoadScene(TEST_SCENE_PATH);

            _spawnAllShipsEvent.RaiseEvent();

            yield return new WaitForEndOfFrame();

            var shipSpawnPoints = GameObject.FindObjectsByType<ShipSpawnPoint>(FindObjectsSortMode.None);
            Assert.AreEqual(shipSpawnPoints.Length, GetShipInScene().Length);
            Assert.IsFalse(_shipBus.HasSailed);
        }

        [UnityTest]
        public IEnumerator SailSomeShip_ThereOnly1ShipInScene()
        {
            _shipBus.IsShipActivated = true;
            _shipBus.HasSailed = false;
            yield return LoadScene(TEST_SCENE_PATH);

            var hero = _gameplayBus.Hero;

            var shipBehaviours = GetShipInScene();
            var someShip = shipBehaviours[0];
            someShip.Interact(hero.gameObject);

            yield return new WaitForSeconds(1f);

            Assert.AreEqual(1, GetShipInScene().Length);
        }

        [UnityTest]
        public IEnumerator AnchorSomeShip_LastPositionSaved()
        {
            _shipBus.IsShipActivated = true;
            _shipBus.HasSailed = false;
            yield return LoadScene(TEST_SCENE_PATH);

            var hero = _gameplayBus.Hero;
            hero.TryGetComponent<HeroShipBehaviour>(out var heroShipBehaviour);

            var shipBehaviours = GetShipInScene();
            var someShip = shipBehaviours[0];

            someShip.Interact(heroShipBehaviour.gameObject);

            yield return new WaitForSeconds(1f);

            hero.transform.position = new Vector3(100, 100, 0);
            heroShipBehaviour.Landing();

            yield return new WaitForSeconds(1f);

            Assert.IsTrue(_shipBus.HasSailed);
            Assert.AreEqual(_shipBus.LastPosition,
                new SerializableVector2(someShip.transform.position));
        }

        private IEnumerator LoadScene(string scenePath)
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath,
                new LoadSceneParameters(LoadSceneMode.Single));
            _sceneLoadedEvent.EventRaised += SetSceneLoaded;
            yield return new WaitUntil(() => _sceneLoaded);
        }

        private ShipBehaviour[] GetShipInScene()
        {
            return GameObject.FindObjectsByType<ShipBehaviour>(FindObjectsInactive.Include,
                FindObjectsSortMode.None);
        }

        [TearDown]
        public void TearDown()
        {
            _shipBus.LastPosition = new SerializableVector2(Vector2.zero);
            _shipBus.IsShipActivated = false;
            _shipBus.HasSailed = false;
            _sceneLoadedEvent.EventRaised -= SetSceneLoaded;
            _sceneLoaded = false;
        }

        private void SetSceneLoaded() => _sceneLoaded = true;
    }
}