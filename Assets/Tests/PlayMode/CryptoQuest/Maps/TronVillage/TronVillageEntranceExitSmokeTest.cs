using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Map;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.CryptoQuest.Maps.TronVillage
{
    public class TronVillageEntranceExitSmokeTest
    {
        private const int FRAMES_TO_WAIT = 360;
        private LoadSceneEventChannelSO _loadMapEvent;

        [UnityTest]
        public IEnumerator TronVillageEntrancesAndExits_SetUpCorrectly()
        {
            const string startupSceneName = "Startup";

            yield return SceneManager.LoadSceneAsync(startupSceneName, LoadSceneMode.Single);

            Assert.That(SceneManager.GetActiveScene().name == startupSceneName);
            var sceneSOGuids = AssetDatabase.FindAssets("t: SceneScriptableObject");
            var framesToWait = FRAMES_TO_WAIT;
            while (framesToWait >= -1)
            {
                framesToWait--;
                yield return null;
            }

            SceneScriptableObject trollVillageSceneSO = new SceneScriptableObject();
            foreach (var guid in sceneSOGuids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var sceneSO = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                if (sceneSO.name == "TronVillageScene")
                    trollVillageSceneSO = sceneSO;
            }

            var eventSOGuids = AssetDatabase.FindAssets("t: LoadSceneEventChannelSO");
            foreach (var guid in eventSOGuids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var loadEventSO = UnityEditor.AssetDatabase.LoadAssetAtPath<LoadSceneEventChannelSO>(path);
                if (loadEventSO.name == "LoadMapEventChannel")
                {
                    _loadMapEvent = loadEventSO;
                    _loadMapEvent.RequestLoad(trollVillageSceneSO);
                }
            }

            framesToWait = 360;
            while (framesToWait >= -1)
            {
                framesToWait--;
                yield return null;
            }

            Assert.That(SceneManager.GetSceneByName("TronVillage").isLoaded);
            var mapExitGOs = GameObject.FindObjectsOfType<MapExit>();
            var mapEntranceGOs = GameObject.FindObjectsOfType<MapEntrance>();

            Assert.NotNull(mapExitGOs);
            Assert.NotNull(mapEntranceGOs);
            int exitCount = 0;
            int entranceCount = 0;
            List<MapPathSO> mapExitPaths = new List<MapPathSO>();
            List<MapPathSO> mapEntrancePaths = new List<MapPathSO>();
            foreach (var mapExitGO in mapExitGOs)
            {
                var mapExit = mapExitGO.GetComponent<MapExit>();
                Assert.NotNull(mapExit.MapPath);
                mapExitPaths.Add(mapExit.MapPath);
                exitCount++;
            }

            foreach (var mapEntranceGO in mapEntranceGOs)
            {
                var mapEntrance = mapEntranceGO.GetComponent<MapEntrance>();
                Assert.NotNull(mapEntrance.MapPath);
                mapEntrancePaths.Add(mapEntrance.MapPath);
                entranceCount++;
            }

            Assert.AreEqual(5, exitCount);
            Assert.AreEqual(5, entranceCount);
            var exitsSet = new HashSet<MapPathSO>(mapExitPaths);
            var entrancesSet = new HashSet<MapPathSO>(mapEntrancePaths);
            bool isEqual = exitsSet.SetEquals(entrancesSet);
            Assert.IsTrue(isEqual);
        }
    }
}