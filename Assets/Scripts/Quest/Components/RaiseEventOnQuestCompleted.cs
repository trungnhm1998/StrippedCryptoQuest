using CryptoQuest.Quest.Authoring;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Components
{
    public class RaiseEventOnQuestCompleted : MonoBehaviour, IQuestConfigure
    {
        [field: SerializeReference]
        public QuestSO Quest { get; set; }

        public bool IsQuestCompleted { get; set; }
        [SerializeField] private UnityEvent _onQuestCompleted;

#if UNITY_EDITOR
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
#endif

#if UNITY_EDITOR
        private void LoadingSceneEditor()
        {
            QuestManager.OnConfigureQuest?.Invoke(this);
        }
#endif

        private void OnEnable()
        {
#if UNITY_EDITOR
            _onSceneLoadedEventChannel.EventRaised += LoadingSceneEditor;
#else
            QuestManager.OnConfigureQuest?.Invoke(this);
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            _onSceneLoadedEventChannel.EventRaised -= LoadingSceneEditor;
#else
            Quest.OnQuestCompleted -= OnCompleted;
#endif
        }

        private void OnCompleted()
        {
            _onQuestCompleted?.Invoke();
        }

        public void Configure()
        {
            if (IsQuestCompleted)
                OnCompleted();

            Quest.OnQuestCompleted += OnCompleted;
        }
    }
}