using System;
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

            SubscribeSetting(_actorSpawnSetting, Spawn);
            SubscribeSetting(_actorDeSpawnSetting, DeSpawn);
        }

        private void OnDisable()
        {
            _onSceneLoadedEventChannel.EventRaised -= ConfigureActors;

            UnsubscribeSetting(_actorSpawnSetting, ActivateSpawnActor);
            UnsubscribeSetting(_actorDeSpawnSetting, ActivateDeSpawnActor);
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

            Destroy(gameObject);
        }

        private void Spawn(bool isQuestCompleted)
        {
            _actorSpawnSetting.OnConfigure -= Spawn;

            if (!isQuestCompleted)
            {
                _actorSpawnSetting.OnQuestCompleted += ActivateSpawnActor;
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
                _actorDeSpawnSetting.OnQuestCompleted += ActivateDeSpawnActor;
                return;
            }

            ActivateDeSpawnActor();
        }

        #region Extensions

        private void SubscribeSetting(ActorSettingSO setting, Action<bool> configureAction)
        {
            if (!setting) return;

            setting.OnConfigure += configureAction;
            setting.Subscribe();
        }

        private void UnsubscribeSetting(ActorSettingSO setting, Action activateAction)
        {
            if (!setting) return;
            
            setting.OnQuestCompleted -= activateAction;
            setting.Unsubscribe();
        }

        #endregion
    }
}