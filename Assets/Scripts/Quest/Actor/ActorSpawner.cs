using System;
using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour, IQuestConfigure
    {
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private ActorSO _actorDef;
        [SerializeField] private Transform _spawnPoint;

        private void OnEnable()
        {
            _onSceneLoadedEventChannel.EventRaised += ConfigureActor;
        }

        private void OnDisable()
        {
            _onSceneLoadedEventChannel.EventRaised -= ConfigureActor;
            if (Quest != null)
                Quest.OnQuestCompleted -= SpawnActor;
        }

        private void ConfigureActor()
        {
            if (Quest == null) SpawnActor();
            else QuestManager.OnConfigureQuest?.Invoke(this);
        }

        private void SpawnActor()
        {
            ActorInfo actor = _actorDef.CreateActor();
            StartCoroutine(actor.Spawn(_spawnPoint));
        }

        [field: SerializeReference] public QuestSO Quest { get; set; }
        public bool IsQuestCompleted { get; set; }

        public void Configure()
        {
            if (IsQuestCompleted)
                SpawnActor();
            Quest.OnQuestCompleted += SpawnActor;
        }
    }
}