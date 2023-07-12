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
            bool hasDialogAssetLoaded = _gameObjectAsyncOps.IsValid() && _gameObjectAsyncOps.Status == AsyncOperationStatus.Succeeded;
            if (hasDialogAssetLoaded)
            {
                DialogPrefabAssetLoaded(_gameObjectAsyncOps);
                return;
            }
            _dialogPrefab.LoadAssetAsync<GameObject>().Completed += DialogPrefabAssetLoaded;
        }

        private AsyncOperationHandle<GameObject> _gameObjectAsyncOps;
        private T _dialogInstance;
        public T DialogInstance => _dialogInstance;

        private void DialogPrefabAssetLoaded(AsyncOperationHandle<GameObject> gameObjectAsyncOps)
        {
            _gameObjectAsyncOps = gameObjectAsyncOps;
            InstantiateDialogPrefab(gameObjectAsyncOps.Result);
        }

        private void InstantiateDialogPrefab(GameObject dialogPrefab)
        {
            var dialog = Instantiate(dialogPrefab, _dialogsContainer);

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

