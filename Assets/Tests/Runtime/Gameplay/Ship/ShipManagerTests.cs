using System.Collections;
using System.Reflection;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Ship;
using CryptoQuest.System.SaveSystem.Loaders;
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
        private VoidEventChannelSO _requestSpawnAllShipsEvent;
        private VoidEventChannelSO _forceSpawnAllShipsEvent;
        private HeroBehaviour _hero;
        private VoidEventChannelSO _sceneLoadedEvent;

        private const string TEST_SCENE_PATH = "Assets/Scenes/WIP/TestShipMap.unity";

        [SetUp]
        public void Setup()
        {
            _shipBus = AssetDatabase.LoadAssetAtPath<ShipBus>(
                "Assets/ScriptableObjects/GameplayBuses/ShipBus.asset");
            _requestSpawnAllShipsEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/Ship/RequestSpawnAllShipsEventChannel.asset");
            _forceSpawnAllShipsEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/Ship/ForceSpawnAllShipsEventChannel.asset");
            _sceneLoadedEvent = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(
                "Assets/ScriptableObjects/Events/SceneManagement/SceneLoadedEventChannel.asset");
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
            _shipBus.CurrentSailState = ESailState.NotSail;
            yield return LoadScene(TEST_SCENE_PATH);

            var shipSpawnPoints = GameObject.FindObjectsByType<ShipSpawnPoint>(FindObjectsSortMode.None);
            Assert.AreEqual(shipSpawnPoints.Length, GetShipInScene().Length);
        }

        [UnityTest]
        public IEnumerator RequestSpawnShip_ShipActivatedAndLanded_ThereOnly1ShipAtPosition()
        {
            _shipBus.LastPosition = new SerializableVector2(new Vector2(100, 100));
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.Landed;
            yield return LoadScene(TEST_SCENE_PATH);

            var ships = GetShipInScene();
            Assert.AreEqual(1, ships.Length);

            Assert.AreEqual(_shipBus.LastPosition.ToVector3(), ships[0].transform.position);
        }

        [UnityTest]
        public IEnumerator RequestSpawnShip_ShipActivatedAndIsSailing_ThereOnly1ShipAtHeroPosition()
        {
            _shipBus.LastPosition = new SerializableVector2(new Vector2(100, 100));
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.Sailing;

            yield return LoadScene(TEST_SCENE_PATH);

            yield return new WaitForSeconds(1f);
            var ships = GetShipInScene();
            Assert.AreEqual(1, ships.Length);
            Assert.AreEqual(new SerializableVector2(ships[0].transform.position),
                new SerializableVector2(_hero.transform.position));
        }

        [UnityTest]
        public IEnumerator ForceRespawnAllShips_AfterShipActivatedAndLanded_ShipsReseted()
        {
            _shipBus.LastPosition = new SerializableVector2(new Vector2(100, 100));
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.Landed;
            yield return LoadScene(TEST_SCENE_PATH);

            _forceSpawnAllShipsEvent.RaiseEvent();
            yield return new WaitForEndOfFrame();

            var shipSpawnPoints = GameObject.FindObjectsByType<ShipSpawnPoint>(FindObjectsSortMode.None);
            Assert.AreEqual(shipSpawnPoints.Length, GetShipInScene().Length);
            Assert.IsTrue(_shipBus.CurrentSailState == ESailState.NotSail);
        }

        [UnityTest]
        public IEnumerator SailSomeShip_ThereOnly1ShipInScene()
        {
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.NotSail;
            yield return LoadScene(TEST_SCENE_PATH);

            var shipBehaviours = GetShipInScene();
            var someShip = shipBehaviours[0];
            someShip.Interact(_hero.gameObject);

            yield return new WaitForSeconds(1f);

            Assert.AreEqual(1, GetShipInScene().Length);
        }

        [UnityTest]
        public IEnumerator AnchorSomeShip_LastPositionSaved()
        {
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.NotSail;
            yield return LoadScene(TEST_SCENE_PATH);

            _hero.TryGetComponent<HeroShipBehaviour>(out var heroShipBehaviour);

            var shipBehaviours = GetShipInScene();
            var someShip = shipBehaviours[0];

            someShip.Interact(heroShipBehaviour.gameObject);

            yield return new WaitForSeconds(.5f);

            _hero.transform.position = new Vector3(100, 100, 0);
            heroShipBehaviour.Landing();

            Assert.IsTrue(_shipBus.CurrentSailState == ESailState.Landed);
            Assert.AreEqual(_shipBus.LastPosition,
                new SerializableVector2(someShip.transform.position));
        }

        private IEnumerator LoadScene(string scenePath)
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(scenePath,
                new LoadSceneParameters(LoadSceneMode.Single));
            _hero = GameObject.FindObjectOfType<HeroBehaviour>();
            _requestSpawnAllShipsEvent.RaiseEvent();
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
            _shipBus.CurrentSailState = ESailState.NotSail;
        }
    }
}