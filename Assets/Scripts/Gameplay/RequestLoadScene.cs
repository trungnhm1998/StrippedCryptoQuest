using System.Collections;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Gameplay
{
    /// <summary>
    /// This script is used to load the scene in the editor.
    /// </summary>
    public class RequestLoadScene : MonoBehaviour
    {
        [SerializeField] private SceneAssetReference _requestScene;

        private IEnumerator Start()
        {
            Scene scene = SceneManager.GetSceneByName(_requestScene.editorAsset.name);
            if (scene.isLoaded) yield break;
            
            yield return _requestScene.LoadSceneAsync(LoadSceneMode.Additive);
        }
    }
}