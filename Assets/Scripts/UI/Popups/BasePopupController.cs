using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Pool;

namespace CryptoQuest.UI.Popups
{
    public abstract class BasePopupController<T> : MonoBehaviour where T : UIPopup
    {
        [SerializeField] protected PopupInputManager _inputManager;
        [SerializeField] private Transform _popupParent;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private float _autoReleaseTimeout = 120f;

        private AsyncOperationHandle<GameObject> _handler;
        // I used pool because there might be many popup show at the same time
        private ObjectPool<T> _popupPool;
        protected readonly List<T> _popups = new();

        public bool IsPopupsEmpty => _popups.Count <= 0;

        private void Awake()
        {
            if (_popupParent == null) _popupParent = transform;
            _popupPool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroyPopup);
        }

        private void OnDisable()
        {
            _popups.Clear();
            ReleaseAssets();
        }

        protected void ShowPopup(Action<T> popupCallback)
        {
            StartCoroutine(CoShowPopup(popupCallback));
        }

        private IEnumerator CoShowPopup(Action<T> popupCallback)
        {
            if (_popups.Count > 0 || (_handler.IsDone && _handler.IsValid()))
            {
                popupCallback?.Invoke(_popupPool.Get());
                yield break;
            }

            _handler = Addressables.LoadAssetAsync<GameObject>(_prefab);
            yield return _handler;
            if (!_handler.IsDone) yield break;
            popupCallback?.Invoke(_popupPool.Get());
        }

        protected void Hide(T popup)
        {
            _popupPool.Release(popup);

            if (IsPopupsEmpty) _inputManager.DisableInput();
        }

        private void ReleaseAssets()
        {
            // If popup empty, release popup asset after _autoReleaseTimeout seconds
            // If there new popup added the release will be cancel 
            if (_popups.Count > 0) return;

            // This should call OnDestroyPopup and destroy all pooled popup
            _popupPool.Dispose();

            if (_handler.IsValid()) 
            {
                Addressables.Release(_handler);
                _prefab.ReleaseAsset();
            }
        }

        private T OnCreate()
        {
            var gameObject = Instantiate(_handler.Result, _popupParent);
            var popup = gameObject.GetComponent<T>();
            return popup;
        }

        private void OnGet(T popup)
        {
            popup.gameObject.SetActive(true);
            _inputManager.EnableInput();

            if (_popups.Contains(popup)) return;
            _popups.Add(popup);
        }

        private void OnRelease(T popup)
        {
            popup.gameObject.SetActive(false);

            _popups.Remove(popup);

            Invoke(nameof(ReleaseAssets), _autoReleaseTimeout);
        }

        private void OnDestroyPopup(T popup)
        {
            Destroy(popup.gameObject);
        }
    }
}