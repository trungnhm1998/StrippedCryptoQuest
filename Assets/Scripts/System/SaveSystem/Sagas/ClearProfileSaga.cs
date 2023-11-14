using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.SaveSystem.Sagas
{
    public class ClearProfileAction : ActionBase { }

    public class ClearProfileSaga : SagaBase<ClearProfileAction>
    {
        [SerializeField] private SceneScriptableObject _titleScene;
        [SerializeField] private LoadSceneEventChannelSO _loadTitleEventChannel;
        [SerializeField] private SceneScriptableObject _defaultStartScene;
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        private bool _isLoadingTitle = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            _sceneLoaded.EventRaised += OnSceneLoaded;
        }

        protected override void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnSceneLoaded;
            base.OnDisable();
        }

        private void OnSceneLoaded()
        {
            _isLoadingTitle = false;
        }

        protected override void HandleAction(ClearProfileAction ctx)
        {
            if (_isLoadingTitle) return;
            var saveSystem = (SaveSystemSO)ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.SaveData.Objects = new();
                _defaultStartScene.SceneReference.ReleaseAsset();
                saveSystem.Save();
            }
            _isLoadingTitle = true;
            _loadTitleEventChannel.RequestLoad(_titleScene);
        }
    }
}