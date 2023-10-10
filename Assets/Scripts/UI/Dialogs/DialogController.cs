using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Dialogs
{
    /// <summary>
    /// The game should interact with dialog through this component for runtime loading
    /// </summary>
    public abstract class DialogController<T> : MonoBehaviour where T : AbstractDialog
    {
        public static DialogController<T> Instance { get; private set; }
        [SerializeField] private Transform _canvasParent;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private float _autoReleaseTimeout = 120f;
        private AsyncOperationHandle<GameObject> _handler;
        public AsyncOperationHandle<GameObject> Handler => _handler;
        private readonly List<T> _dialogs = new();
        private readonly Dictionary<T, Coroutine> _releaseCoroutines = new();
        private readonly Dictionary<T, int> _cachedDialogs = new();

        private void Awake()
        {
            Instance = this;
            if (_canvasParent == null) _canvasParent = transform;
        }

        public void Instantiate(Action<T> instantiatedCallback = null, bool autoRelease = true,
            float releaseTimeout = 0)
            => StartCoroutine(CoInstantiate(instantiatedCallback, autoRelease, releaseTimeout));

        public IEnumerator CoInstantiate(Action<T> instantiatedCallback = null, bool autoRelease = true,
            float releaseTimeout = 0)
        {
            if (_handler.IsDone && _handler.IsValid())
            {
                var dialog = Instantiate(_handler.Result, gameObject.transform);
                instantiatedCallback?.Invoke(dialog.GetComponent<T>());
                yield break;
            }

            yield return LoadDialogPrefab(instantiatedCallback, autoRelease, releaseTimeout);
        }

        public void Release(T dialogToRelease)
        {
            if (dialogToRelease == null) return;
            if (!_cachedDialogs.ContainsKey(dialogToRelease)) return;
            var index = _cachedDialogs[dialogToRelease];
            _dialogs.RemoveAt(index);
            _cachedDialogs.Remove(dialogToRelease);
            if (_releaseCoroutines.TryGetValue(dialogToRelease, out var coroutine))
            {
                if (coroutine != null) StopCoroutine(coroutine);
            }

            _releaseCoroutines.Remove(dialogToRelease);
            Destroy(dialogToRelease.gameObject);
        }

        private void ReleaseAssets()
        {
            if (_dialogs.Count > 0) return;
            if (_handler.IsValid()) 
            {
                Addressables.Release(_handler);
                _prefab.ReleaseAsset();
            }
        }

        private IEnumerator LoadDialogPrefab(Action<T> createdCallback, bool autoRelease, float releaseTimeout)
        {
            if (!_handler.IsValid())
            {
                _handler = _prefab.LoadAssetAsync<GameObject>();
            }
            yield return _handler;
            if (!_handler.IsDone) yield break;

            var dialogGameObject = Instantiate(_handler.Result, gameObject.transform);
            var dialog = dialogGameObject.GetComponent<T>();
            _dialogs.Add(dialog);
            _cachedDialogs[dialog] = _dialogs.Count - 1;
            _releaseCoroutines[dialog] = autoRelease ? StartCoroutine(CoAutoRelease(dialog, releaseTimeout)) : null;
            createdCallback?.Invoke(dialog);
        }

        private IEnumerator CoAutoRelease(T dialog, float releaseTimeout)
        {
            yield return new WaitForSeconds(releaseTimeout > 0 ? releaseTimeout : _autoReleaseTimeout);
            if (dialog.Content.activeSelf)
            {
                _releaseCoroutines[dialog] = StartCoroutine(CoAutoRelease(dialog, releaseTimeout));
                yield break;
            }

            Release(dialog);
        }
    }
}