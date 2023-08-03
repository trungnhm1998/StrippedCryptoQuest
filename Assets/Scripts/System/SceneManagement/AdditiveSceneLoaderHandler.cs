using CryptoQuest.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI.FadeController;
using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    public class AdditiveSceneLoaderHandler : AdditiveGameSceneLoader
    {
        [SerializeField] private ConfigSOEventChannelSO _onSetConfigEventChannel;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onSetConfigEventChannel.EventRaised += SetFadeConfig;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _onSetConfigEventChannel.EventRaised -= SetFadeConfig;
        }

        private void SetFadeConfig(FadeConfigSO configSo)
        {
            _currentConfigUsed = configSo;
        }
    }
}