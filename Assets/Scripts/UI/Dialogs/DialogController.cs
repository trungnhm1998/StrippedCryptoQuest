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
        [SerializeField] private AssetReference _prefab;
        private AsyncOperationHandle<GameObject> _handler;

        private void Awake()
        {
            Instance = this;
        }

        public void CreateDialog(Action<T> createdCallback)
        {
            if (_handler.IsDone && _handler.IsValid())
            {
                var dialog = Instantiate(_handler.Result, gameObject.transform);
                createdCallback?.Invoke(dialog.GetComponent<T>());
                return;
            }

            StartCoroutine(LoadDialogPrefab(createdCallback));
        }

        public void ReleaseDialog()
        {
            if (_handler.IsValid())
            {
                Addressables.Release(_handler);
            }
        }

        private IEnumerator LoadDialogPrefab(Action<T> createdCallback)
        {
            _handler = _prefab.LoadAssetAsync<GameObject>();
            yield return _handler;
            if (!_handler.IsDone) yield break;

            var dialog = Instantiate(_handler.Result, gameObject.transform);
            createdCallback?.Invoke(dialog.GetComponent<T>());
        }
    }
}