using System.Collections.Generic;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuestEditor.EditorTool
{
    public class TeleportEditorCheat : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public LoadSceneEventChannelSO SceneLoadedEventChannelSO;

        private readonly List<SceneScriptableObject> _sceneScriptableObjects = new List<SceneScriptableObject>();
        private bool _showSceneList;

        private void OnGUI()
        {
            _showSceneList = GUILayout.Toggle(_showSceneList, "Show add effects");
            if (!_showSceneList) return;


            if (_sceneScriptableObjects.Count > 0)
            {
                foreach (var sceneScriptableObject in _sceneScriptableObjects)
                {
                    if (GUILayout.Button($"Teleport to {sceneScriptableObject.name}"))
                    {
                        OnLoadingRequested(sceneScriptableObject);
                    }
                }
            }
        }

        private void OnLoadingRequested(SceneScriptableObject sceneScriptableObject)
        {
            SceneLoadedEventChannelSO.RequestLoad(sceneScriptableObject);
        }

        private void Start()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:SceneScriptableObject");

            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var sceneSO = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(path);
                if (sceneSO.SceneType == SceneScriptableObject.Type.Location)
                    _sceneScriptableObjects.Add(sceneSO);
            }
        }
#endif
    }
}