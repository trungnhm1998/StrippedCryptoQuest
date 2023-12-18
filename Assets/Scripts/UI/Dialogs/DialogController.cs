using System;
using System.Collections;
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
        private AsyncOperationHandle<GameObject> _handler;
        private T _dialog;
        public T GetDialog() => _dialog;
        public AsyncOperationHandle<GameObject> Handler => _handler;

        private void Awake()
        {
            Instance = this;
            if (_canvasParent == null) _canvasParent = transform;
        }
        
        public void InstantiateAsync(Action<T> instantiatedCallback = null) => StartCoroutine(CoInstantiate(instantiatedCallback));

        public IEnumerator CoInstantiate(Action<T> instantiatedCallback = null)
        {
            if (_dialog != null)
            {
                instantiatedCallback?.Invoke(_dialog);
                yield break;
            }
            
            if (!_handler.IsValid())
                _handler = _prefab.InstantiateAsync(_canvasParent);
            yield return _handler;
            _dialog = _handler.Result.GetComponent<T>();
            instantiatedCallback?.Invoke(_dialog);
        }

        public void Release(T dialogToRelease)
        {
            if (dialogToRelease == null) return;
            dialogToRelease.Hide();
        }
    }
}