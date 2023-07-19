using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Events/Load Scene Event Channel")]
    public class LoadSceneEventChannelSO : ScriptableObject
    {
        public UnityAction<SceneScriptableObject> LoadingRequested;

        public void RequestLoad(SceneScriptableObject sceneScriptableObject)
        {
            OnRequestLoad(sceneScriptableObject);
        }

        private void OnRequestLoad(SceneScriptableObject sceneScriptableObject)
        {
            if (sceneScriptableObject == null)
            {
                Debug.LogWarning("A request for loading scene has been made, but no scene was provided.");
                return;
            }

            if (LoadingRequested == null)
            {
                Debug.LogWarning("A request for loading scene has been made, but no one listening.");
                return;
            }

            LoadingRequested.Invoke(sceneScriptableObject);
        }
    }
}