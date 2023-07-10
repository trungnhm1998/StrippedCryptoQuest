using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CryptoQuest.UI.Dialogs
{
    public class DialogFactory : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjectAssetReference<DialogSO> _loadDialogEventChannelSO;

        public void LoadAssetDialog()
        {
            _loadDialogEventChannelSO.LoadAssetAsync<DialogSO>().Completed += OnLoadDialogComplete;
        }

        public void OnLoadDialogComplete(AsyncOperationHandle<DialogSO> tmp)
        {
            
        }
    }
}
