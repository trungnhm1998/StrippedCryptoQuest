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
            if (_actorSpawnSetting) _actorSpawnSetting.QuestToTrack.OnQuestCompleted -= ActivateSpawnActor;
            if (_actorDeSpawnSetting) _actorDeSpawnSetting.QuestToTrack.OnQuestCompleted -= ActivateDeSpawnActor;
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
            if (!_spawnPoint) return;

            ActorInfo actor = _actorDef.CreateActor();

            StartCoroutine(actor.Spawn(_spawnPoint));
        }

        private void ActivateDeSpawnActor()
        {
            if (!_spawnPoint) return;

            Destroy(gameObject, 1);
        }

        private void Spawn(bool isQuestCompleted)
        {
            _actorSpawnSetting.OnConfigure -= Spawn;

            if (!isQuestCompleted)
            {
                _actorSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateSpawnActor;
                return;
            }

            ActivateSpawnActor();
        }

        private void DeSpawn(bool isQuestCompleted)
        {
            _actorDeSpawnSetting.OnConfigure -= DeSpawn;

            if (!isQuestCompleted)
            {
                InitSpawnSetting();
                _actorDeSpawnSetting.QuestToTrack.OnQuestCompleted += ActivateDeSpawnActor;
                return;
            }

            ActivateDeSpawnActor();
        }
    }
}