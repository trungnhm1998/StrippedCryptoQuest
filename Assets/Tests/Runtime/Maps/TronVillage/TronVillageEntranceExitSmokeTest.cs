using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.Runtime.Maps.TronVillage
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
            var goToGOs = GameObject.FindObjectsOfType<GoTo>();
            var goFromGOs = GameObject.FindObjectsOfType<GoFrom>();

            Assert.NotNull(goToGOs);
            Assert.NotNull(goFromGOs);
            int exitCount = 0;
            int entranceCount = 0;
            List<MapPathSO> goToPaths = new List<MapPathSO>();
            List<MapPathSO> goFromPaths = new List<MapPathSO>();
            foreach (var goToGameOject in goToGOs)
            {
                var goTo = goToGameOject.GetComponent<GoTo>();
                Assert.NotNull(goTo.MapPath);
                goToPaths.Add(goTo.MapPath);
                exitCount++;
            }

            foreach (var goFromGameObject in goFromGOs)
            {
                var goFrom = goFromGameObject.GetComponent<GoFrom>();
                Assert.NotNull(goFrom.MapPath);
                goFromPaths.Add(goFrom.MapPath);
                entranceCount++;
            }

            Assert.AreEqual(5, exitCount);
            Assert.AreEqual(5, entranceCount);
            var exitsSet = new HashSet<MapPathSO>(goToPaths);
            var entrancesSet = new HashSet<MapPathSO>(goFromPaths);
            bool isEqual = exitsSet.SetEquals(entrancesSet);
            Assert.IsTrue(isEqual);
        }
    }
}