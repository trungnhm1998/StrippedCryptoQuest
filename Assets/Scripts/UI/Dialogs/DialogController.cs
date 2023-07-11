using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.UI.Dialogs
{
    public abstract class DialogController<T> : MonoBehaviour where T : ModalWindow<T>
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

            _dialogPrefab.LoadAssetAsync<GameObject>().Completed += InstantiateDialogPrefab;
        }

        private T _dialogInstance;
        public T DialogInstance => _dialogInstance;

        protected void InstantiateDialogPrefab(AsyncOperationHandle<GameObject> args)
        {
            var dialog = Instantiate(args.Result);

            dialog.transform.SetParent(_dialogsContainer);

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

