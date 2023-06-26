using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest;
using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;

public class TeleportEditorCheat : MonoBehaviour
{
#if UNITY_EDITOR || DEV_ENV
    private List<SceneScriptableObject> _sceneScriptableObjects = new List<SceneScriptableObject>();
    public LoadSceneEventChannelSO sceneLoadedEventChannelSO;
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
        sceneLoadedEventChannelSO.RequestLoad(sceneScriptableObject);
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
