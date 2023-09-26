using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

        private void OnEnable()
        {
            _sceneLoadedEvent.EventRaised += SceneLoaded;
        }

        private void OnDisable()
        {
            _sceneLoadedEvent.EventRaised -= SceneLoaded;
        }

        private void SceneLoaded()
        {
            QuestManager.OnTriggerQuest?.Invoke(_quest); 
        }
    }
}