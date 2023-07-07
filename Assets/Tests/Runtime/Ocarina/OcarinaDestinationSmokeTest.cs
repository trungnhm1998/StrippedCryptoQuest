using System.Collections;
using System.Collections.Generic;
using CryptoQuest;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Tests.Runtime.Ocarina
{
    public class OcarinaDestinationSmokeTest
    {
        private const int FRAMES_TO_WAIT = 360;
        private LoadSceneEventChannelSO _loadMapEvent;

        [UnityTest]
        public IEnumerator OcarinaDestinations_SetUpCorrectly()
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

            SceneScriptableObject worldMapSceneSO = new SceneScriptableObject();
            foreach (var guid in sceneSOGuids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var sceneSO = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                if (sceneSO.name == "WorldMapScene")
                    worldMapSceneSO = sceneSO;
            }

            var eventSOGuids = AssetDatabase.FindAssets("t: LoadSceneEventChannelSO");
            foreach (var guid in eventSOGuids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var loadEventSO = UnityEditor.AssetDatabase.LoadAssetAtPath<LoadSceneEventChannelSO>(path);
                if (loadEventSO.name == "LoadMapEventChannel")
                {
                    _loadMapEvent = loadEventSO;
                    _loadMapEvent.RequestLoad(worldMapSceneSO);
                }
            }

            framesToWait = 500;
            while (framesToWait >= -1)
            {
                framesToWait--;
                yield return null;
            }

            Assert.That(SceneManager.GetSceneByName("WorldMap").isLoaded);
            List<MapPathSO> destinationPaths = new List<MapPathSO>();
            var goFromGOs = GameObject.FindObjectsOfType<GoFrom>();
            foreach (var goFromGO in goFromGOs)
            {
                if (goFromGO.gameObject.name.Contains("Ocarina"))
                {
                    var goFrom = goFromGO.GetComponent<GoFrom>();
                    destinationPaths.Add(goFrom.MapPath);
                }
            }

            List<MapPathSO> ocarinaPathSO = new List<MapPathSO>();
            var ocarinaDataGuids = AssetDatabase.FindAssets("t: OcarinaDataSO");
            var ocarinaDataSOpath = UnityEditor.AssetDatabase.GUIDToAssetPath(ocarinaDataGuids[0]);
            var ocarinaDataSO = UnityEditor.AssetDatabase.LoadAssetAtPath<OcarinaDataSO>(ocarinaDataSOpath);
            foreach (var ocarinaData in ocarinaDataSO.ocarinaDataList)
            {
                ocarinaPathSO.Add(ocarinaData.path);
            }

            var ocarinaPathSet = new HashSet<MapPathSO>(ocarinaPathSO);
            var destinationPathSet = new HashSet<MapPathSO>(destinationPaths);
            bool areEqual = ocarinaPathSet.SetEquals(destinationPathSet);
            Assert.IsTrue(areEqual);
        }
    }
}