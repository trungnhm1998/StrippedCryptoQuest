using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.CutScene
{
    public class CutSceneSceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _sceneToLoad;

        [Header("Raise event")]
        [SerializeField] private LoadSceneEventChannelSO _sceneLoadChannel;

        public void LoadScene() => _sceneLoadChannel.RequestLoad(_sceneToLoad);
    }
}