using CryptoQuest.Core;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Actions;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Sagas
{
    public class ClearProfileAction : ActionBase { }

    public class ClearProfileSaga : SagaBase<ClearProfileAction>
    {
        [SerializeField] private SceneScriptableObject _titleScene;
        [SerializeField] private LoadSceneEventChannelSO _loadTitleEventChannel;
        [SerializeField] private SceneScriptableObject _defaultStartScene;

        private bool _isLoadingTitle = false;

        protected override void HandleAction(ClearProfileAction ctx)
        {
            if (_isLoadingTitle) return;
            var saveSystem = (SaveSystemSO)ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.SaveData.Objects = new();
                ActionDispatcher.Dispatch(new SaveSceneAction(_defaultStartScene));              
            }
            _isLoadingTitle = true;
            _loadTitleEventChannel.RequestLoad(_titleScene);
            _isLoadingTitle = false;
        }
    }
}