using System;
using Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace Core.SceneManagementSystem.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Events/Load Scene Event Channel")]
    public class LoadSceneEventChannelSO : ScriptableObject
    {
        public Action<SceneScriptableObject> LoadingRequested;

        public bool OnRaiseEvent(SceneScriptableObject sceneScriptableObject)
        {
            if (sceneScriptableObject == null)
            {
                Debug.LogWarning("A request for loading scene has been made, but no scene was provided.");
                return false;
            }

            if (LoadingRequested == null)
            {
                Debug.LogWarning("A request for loading scene has been made, but no one listening.");
                return false;
            }

            LoadingRequested.Invoke(sceneScriptableObject);
            return true;
        }
    }
}