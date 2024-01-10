using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class ComponentLoader : ILoader
    {
        [SerializeField] private GameObject _loaderContainer;

        private ILoader[] _loaders;

        public IEnumerator LoadAsync()
        {
            _loaders ??= _loaderContainer.GetComponents<ILoader>();
            foreach (var loader in _loaders)
                yield return loader.LoadAsync();
        }

        public void Load()
        {
            _loaders ??= _loaderContainer.GetComponents<ILoader>();
            foreach (var loader in _loaders)
                loader.LoadAsync();
        }
    }
}