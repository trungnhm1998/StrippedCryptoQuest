using System;
using System.Collections.Generic;
using CommandTerminal;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.Cheat
{
    public class TeleportCheats : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Location
        {
            public string Name;
            public string Guid;
        }

        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSO;
        [SerializeField] private Location[] _locations;

        private Dictionary<string, string> _locationDictionary = new();

        private void OnValidate()
        {
#if UNITY_EDITOR
            var paths = AssetDatabase.FindAssets("t:SceneScriptableObject");
            var locations = new List<Location>();
            for (var i = 0; i < paths.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(paths[i]);
                var scene = AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                if (scene.SceneType != SceneScriptableObject.Type.Location || path.ToLower().Contains("wip")) continue;
                locations.Add(new Location
                {
                    Name = scene.name.ToLower(),
                    Guid = scene.Guid
                });
            }

            _locations = locations.ToArray();
            EditorUtility.SetDirty(this);
#endif
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("tp", TriggerTeleport, 1, 1, "tp CardanoBar, teleport to CardanoBar");

            foreach (var location in _locations)
            {
                _locationDictionary.Add(location.Name, location.Guid);
                Terminal.Autocomplete.Register(location.Name);
            }
        }

        private void TriggerTeleport(CommandArg[] args)
        {
            var sceneName = args[0].String.ToLower();
            if (!_locationDictionary.TryGetValue(sceneName, out var guid))
            {
                Debug.LogWarning($"Scene {sceneName} not found");
                return;
            }

            var handle = Addressables.LoadAssetAsync<SceneScriptableObject>(guid);
            handle.Completed += LoadScene;
        }

        private void LoadScene(AsyncOperationHandle<SceneScriptableObject> asyncOperationHandle)
        {
            if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded) return;
            if (asyncOperationHandle.Result != null)
                _loadSceneEventChannelSO.RequestLoad(asyncOperationHandle.Result);
        }
    }
}