using System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class PlayQuestCutscene : PlayQuestCutsceneBase
    {
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += SceneLoaded;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= SceneLoaded;
        }

        /// <summary>
        /// Only check the quest after the scene loaded
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SceneLoaded()
        {
            PlayCutscene();
        }
    }
}