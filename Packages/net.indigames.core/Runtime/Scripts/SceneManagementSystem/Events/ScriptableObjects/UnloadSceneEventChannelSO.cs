using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scene Management/Events/Unload Scene Event Channel")]
    public class UnloadSceneEventChannelSO : ScriptableObject
    {
        public UnityAction<SceneScriptableObject> UnloadRequested;

        public void RequestUnload(SceneScriptableObject sceneScriptableObject)
        {
            OnRequestUnload(sceneScriptableObject);
        }

        private void OnRequestUnload(SceneScriptableObject sceneScriptableObject)
        {
            if (sceneScriptableObject == null)
            {
                Debug.LogWarning("A request for unload scene has been made, but no scene was provided.");
                return;
            }

            if (UnloadRequested == null)
            {
                Debug.LogWarning("A request for unload scene has been made, but no one listening.");
                return;
            }

            UnloadRequested.Invoke(sceneScriptableObject);
        }
    }
}