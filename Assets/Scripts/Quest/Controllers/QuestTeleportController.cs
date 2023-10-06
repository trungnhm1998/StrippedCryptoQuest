using System.Collections.Generic;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Controllers
{
    public class QuestTeleportController : MonoBehaviour
    {
        [SerializeField] private LoadSceneEventChannelSO _loadNextSceneEventChannelSO;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannelSO;
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannelSO;
        private Dictionary<SceneScriptableObject, TeleportQuestInfo> _currentlyProcessTeleportQuests = new();
        private TeleportQuestSO _teleportQuestSO;

        private void OnEnable()
        {
            _loadNextSceneEventChannelSO.LoadingRequested += TeleportRequested;
        }

        private void OnDisable()
        {
            _loadNextSceneEventChannelSO.LoadingRequested -= TeleportRequested;
        }

        public void GiveQuest(TeleportQuestInfo questInfo)
        {
            var destination = questInfo.Data.Destination;
            _currentlyProcessTeleportQuests.TryAdd(destination, questInfo);
        }

        private void TeleportRequested(SceneScriptableObject scene)
        {
            if (!_currentlyProcessTeleportQuests.ContainsKey(scene)) return;

            var questInfo = _currentlyProcessTeleportQuests[scene];
            _teleportQuestSO = questInfo.Data;
            _onSceneLoadedEventChannelSO.EventRaised += OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            _onSceneLoadedEventChannelSO.EventRaised -= OnSceneLoaded;
            _triggerQuestEventChannelSO.RaiseEvent(_teleportQuestSO);
        }
    }
}