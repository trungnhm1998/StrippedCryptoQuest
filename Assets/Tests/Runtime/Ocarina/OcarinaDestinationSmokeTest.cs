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
        private readonly WaitForSeconds SECONDS_TO_WAIT = new WaitForSeconds(6);
        private LoadSceneEventChannelSO _loadMapEvent;
        private string STARTUP_SCENE_NAME = "Startup";

        [UnityTest]
        public IEnumerator OcarinaDestinations_SetUpCorrectly()
        {
            yield return SceneManager.LoadSceneAsync(STARTUP_SCENE_NAME, LoadSceneMode.Single);

            Assert.That(SceneManager.GetActiveScene().name == STARTUP_SCENE_NAME);
            SceneScriptableObject worldMapSceneSo = OcarinaDestinationSmokeTest.GetWorldMapScene();

            yield return SECONDS_TO_WAIT;
            _loadMapEvent = GetLoadMapEventSO();
            _loadMapEvent.RequestLoad(worldMapSceneSo);

            yield return SECONDS_TO_WAIT;

            Assert.That(SceneManager.GetSceneByName("WorldMap").isLoaded);
            List<MapPathSO> destinationPaths = new(GetOcarinaDestinationPaths());

            List<MapPathSO> ocarinaPaths = GetOcarinaPathsInData();

            HashSet<MapPathSO> ocarinaPathSet = new(ocarinaPaths);
            HashSet<MapPathSO> destinationPathSet = new(destinationPaths);
            bool areEqual = ocarinaPathSet.SetEquals(destinationPathSet);
            Assert.IsTrue(areEqual);
        }

        private static List<MapPathSO> GetOcarinaPathsInData()
        {
            List<MapPathSO> ocarinaPathSo = new();
            string[] ocarinaDataGuids = AssetDatabase.FindAssets("t: OcarinaLocations");
            string ocarinaDataSOpath = UnityEditor.AssetDatabase.GUIDToAssetPath(ocarinaDataGuids[0]);
            OcarinaLocations ocarinaDataSo =
                UnityEditor.AssetDatabase.LoadAssetAtPath<OcarinaLocations>(ocarinaDataSOpath);
            foreach (OcarinaLocations.Location ocarinaLocation in ocarinaDataSo.Locations)
            {
                ocarinaPathSo.Add(ocarinaLocation.Path);
            }

            return ocarinaPathSo;
        }

        private static List<MapPathSO> GetOcarinaDestinationPaths()
        {
            GoFrom[] goFromGOs = GameObject.FindObjectsOfType<GoFrom>();
            List<MapPathSO> destinationPaths = new();
            foreach (GoFrom goFromGO in goFromGOs)
            {
                if (goFromGO.gameObject.name.Contains("Ocarina"))
                {
                    GoFrom goFrom = goFromGO.GetComponent<GoFrom>();
                    destinationPaths.Add(goFrom.MapPath);
                }
            }

            return destinationPaths;
        }

        private LoadSceneEventChannelSO GetLoadMapEventSO()
        {
            string[] eventSOGuids = AssetDatabase.FindAssets("t: LoadSceneEventChannelSO");
            foreach (string guid in eventSOGuids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                LoadSceneEventChannelSO loadEventSO =
                    UnityEditor.AssetDatabase.LoadAssetAtPath<LoadSceneEventChannelSO>(path);
                if (loadEventSO.name == "LoadMapEventChannel")
                {
                    return loadEventSO;
                }
            }

            return null;
        }

        private static SceneScriptableObject GetWorldMapScene()
        {
            string[] sceneSOGuids = AssetDatabase.FindAssets("t: SceneScriptableObject");
            SceneScriptableObject worldMapSceneSO = new();
            foreach (string guid in sceneSOGuids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                SceneScriptableObject sceneSO = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                if (sceneSO.name == "WorldMapScene")
                    worldMapSceneSO = sceneSO;
            }

            return worldMapSceneSO;
        }
    }
}