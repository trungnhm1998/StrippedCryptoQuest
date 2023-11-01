using System.Collections;
using CryptoQuest.Core;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.SceneLoad
{
    public class PostSceneLoadedAction : ActionBase
    {
    }

    public class SceneLoaderDispatch : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<VoidEventChannelSO> _sceneLoadedEventAsset;
        private VoidEventChannelSO _sceneLoadedEvent;

        private IEnumerator Start()
        {
            var handle = _sceneLoadedEventAsset.LoadAssetAsync();
            yield return handle;
            _sceneLoadedEvent = handle.Result;
            _sceneLoadedEvent.EventRaised += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            ActionDispatcher.Dispatch(new PostSceneLoadedAction());
        }
    }
}