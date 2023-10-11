using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private Transform _spawnPoint;

        [Header("Actor Settings")]
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
            ActorSettingSO actorSetting = _actorDeSpawnSetting != null
                ? _actorDeSpawnSetting
                : _actorSpawnSetting;

            QuestManager.OnConfigureQuest?.Invoke(actorSetting);
        }

        private void InitSpawnSetting()
        {
            if (_actorSpawnSetting != null)
            {
                QuestManager.OnConfigureQuest?.Invoke(_actorSpawnSetting);
                return;
            }

            ActivateSpawnActor();
        }


        private void ActivateSpawnActor()
        {
            ActorInfo actor = _actorDef.CreateActor();
            if (_spawnPoint.transform == null) return;

            StartCoroutine(actor.Spawn(_spawnPoint.transform));
        }

        private void ActivateDeSpawnActor()
        {
            if (transform.gameObject == null) return;

            Destroy(transform.gameObject);
        }

        private void Spawn(bool isQuestCompleted)
        {
            if (isQuestCompleted) ActivateSpawnActor();
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