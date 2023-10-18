using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private Transform _spawnPoint;

        [Header("Actor Settings")] [SerializeField]
        private ActorSO _actorDef;

        [SerializeField] private ActorSettingSO _actorSpawnSetting;
        [SerializeField] private ActorSettingSO _actorDeSpawnSetting;

        private bool _isSpawned;
        private bool _isDeSpawned;

        private void OnEnable()
        {
            _onSceneLoadedEventChannel.EventRaised += ConfigureActors;
            if (_actorSpawnSetting) _actorSpawnSetting.OnConfigure += Spawn;
            if (_actorDeSpawnSetting) _actorDeSpawnSetting.OnConfigure += DeSpawn;
        }

        private void OnDisable()
        {
            _onSceneLoadedEventChannel.EventRaised -= ConfigureActors;
            //TODO: these codes smell, need refactor   
            if (_actorSpawnSetting)
            {
                _actorSpawnSetting.OnConfigure -= Spawn;
                _actorSpawnSetting.QuestToTrack.OnQuestCompleted -= ActivateSpawnActor;
            }

            if (_actorDeSpawnSetting)
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
            if (_actorSpawnSetting)
            {
                QuestManager.OnConfigureQuest?.Invoke(_actorSpawnSetting);
                return;
            }

            ActivateSpawnActor();
        }

        private void ActivateSpawnActor()
        {
            StopAllCoroutines();
            ActorInfo actor = _actorDef.CreateActor();

            if (!_spawnPoint && _isSpawned) return;

            StartCoroutine(actor.Spawn(_spawnPoint.transform));

            _isSpawned = true;
        }

        private void ActivateDeSpawnActor()
        {
            if (!_spawnPoint && _isDeSpawned) return;

            Destroy(transform.gameObject, 1);
            _isSpawned = true;
        }

        private void Spawn(bool isQuestCompleted)
        {
            if (!isQuestCompleted)
            {
                _actorSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateSpawnActor;
                return;
            }

            ActivateSpawnActor();
        }

        private void DeSpawn(bool isQuestCompleted)
        {
            if (!isQuestCompleted)
            {
                _actorDeSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateDeSpawnActor;
                InitSpawnSetting();
                return;
            }

            ActivateDeSpawnActor();
        }
    }
}