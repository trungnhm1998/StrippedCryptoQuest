using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Dialogs
{
    public abstract class AbstractDialogController<T> : MonoBehaviour where T : ModalWindow<T>
    {
        [SerializeField] private AssetReference _dialogPrefab;
        [SerializeField] private Transform _dialogsContainer;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();

        protected void LoadAssetDialog()
        {
            if (_gameObjectAsyncOps.IsValid() && _gameObjectAsyncOps.Status == AsyncOperationStatus.Succeeded)
            {
                InstantiateDialogPrefab(_gameObjectAsyncOps);
                return;
            }
            _dialogPrefab.LoadAssetAsync<GameObject>().Completed += InstantiateDialogPrefab;
        }

        private T _dialogInstance;
        private AsyncOperationHandle<GameObject> _gameObjectAsyncOps;

        public T DialogInstance => _dialogInstance;

        protected void InstantiateDialogPrefab(AsyncOperationHandle<GameObject> gameObjectAsyncOps)
        {
            _gameObjectAsyncOps = gameObjectAsyncOps;
            var dialog = Instantiate(gameObjectAsyncOps.Result, _dialogsContainer);

            _dialogInstance = dialog.GetComponent<T>();
            SetupDialog(_dialogInstance);
        }

        /// <summary>
        /// Dialog life cycle
        /// </summary>
        /// <param name="dialog"></param>
        protected abstract void SetupDialog(T dialog);

    }
}

