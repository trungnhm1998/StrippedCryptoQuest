using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Sagas
{
    public class GoToTitleAction : ActionBase { }

    public class GoToTitleSaga : SagaBase<GoToTitleAction>
    {
        [SerializeField] private SceneScriptableObject _titleScene;
        [SerializeField] private LoadSceneEventChannelSO _loadTitleEventChannel;
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

        protected override void HandleAction(GoToTitleAction ctx)
        {
            if (_isLoadingTitle) return;
            _isLoadingTitle = true;
            _loadTitleEventChannel.RequestLoad(_titleScene);
        }

        private void OnSceneLoaded()
        {
            _isLoadingTitle = false;
        }
    }
}