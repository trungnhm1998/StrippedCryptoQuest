using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;

        [SerializeField] private QuestTriggerEventChannelSO questTriggerEventChannel;

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
            if (_quest == null) return;
            
            questTriggerEventChannel.RaiseEvent(_quest);
        }
    }
}