using System;
using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest
{
    public class SceneLoaderDispatch : MonoBehaviour
    {
        public static Action SceneLoaded;
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
            SceneLoaded?.Invoke();
        }
    }
}