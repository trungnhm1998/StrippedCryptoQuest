using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private ActorSO _actorDef;
        [SerializeField] private ActorSettingSO _actorSpawnSetting;
        [SerializeField] private ActorSettingSO _actorDeSpawnSetting;

        private void OnEnable()
        {
            _onSceneLoadedEventChannel.EventRaised += ConfigureActors;
            if (_actorSpawnSetting != null) _actorSpawnSetting.OnConfigure += Spawn;
            if (_actorDeSpawnSetting != null) _actorDeSpawnSetting.OnConfigure += DeSpawn;
        }

        private void OnDisable()
        {
            _onSceneLoadedEventChannel.EventRaised -= ConfigureActors;
            //TODO: these codes smell, need refactor   
            if (_actorSpawnSetting != null)
            {
                _actorSpawnSetting.OnConfigure -= Spawn;
                _actorSpawnSetting.QuestToTrack.OnQuestCompleted -= ActivateSpawnActor;
            }

            if (_actorDeSpawnSetting != null)
            {
                _actorDeSpawnSetting.OnConfigure -= DeSpawn;
                _actorDeSpawnSetting.QuestToTrack.OnQuestCompleted -= ActivateDeSpawnActor;
            }
        }

        private void ConfigureActors()
        {
            QuestManager.OnConfigureQuest?.Invoke(_actorDeSpawnSetting != null
                ? _actorDeSpawnSetting
                : _actorSpawnSetting);
        }

        private void InitSpawnSetting()
        {
            if (_actorSpawnSetting == null)
                ActivateSpawnActor();
            else
                QuestManager.OnConfigureQuest?.Invoke(_actorSpawnSetting);
        }


        private void ActivateSpawnActor()
        {
            ActorInfo actor = _actorDef.CreateActor();
            if (transform != null)
                StartCoroutine(actor.Spawn(transform));
        }

        private void ActivateDeSpawnActor()
        {
            if (transform.gameObject != null)
                Destroy(transform.gameObject);
        }

        private void Spawn(bool isQuestCompleted)
        {
            if (isQuestCompleted)
                ActivateSpawnActor();
            _actorSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateSpawnActor;
        }

        private void DeSpawn(bool isQuestCompleted)
        {
            if (isQuestCompleted)
            {
                ActivateDeSpawnActor();
                return;
            }

            _actorDeSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateDeSpawnActor;
            InitSpawnSetting();
        }
    }
}