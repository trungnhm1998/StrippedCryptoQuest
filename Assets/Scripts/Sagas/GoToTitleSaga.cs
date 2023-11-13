using CryptoQuest.Core;
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

        private bool _isLoadingTitle = false;

        protected override void HandleAction(GoToTitleAction ctx)
        {
            if (_isLoadingTitle) return;
            _isLoadingTitle = true;
            _loadTitleEventChannel.RequestLoad(_titleScene);
            _isLoadingTitle = false;
        }
    }
}