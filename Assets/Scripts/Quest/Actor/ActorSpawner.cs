using CryptoQuest.Quest.Components;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private Transform _spawnPoint;

        [Header("Actor Settings")] [SerializeField]
        private ActorSO _actorDef;

        [FormerlySerializedAs("_actorSpawnSetting")] [SerializeField]
        private ActorSettingSO _actorSpawnSettingSO;

        [FormerlySerializedAs("_actorDeSpawnSetting")] [SerializeField]
        private ActorSettingSO _actorDeSpawnSettingSO;

        private ActorSettingInfo _spawnSetting;
        private ActorSettingInfo _vanishSetting;

        private ActorInfo _actor;

        private void OnEnable()
        {
            _onSceneLoadedEventChannel.EventRaised += ConfigureActors;

            if (_actorSpawnSettingSO) _spawnSetting = _actorSpawnSettingSO.CreateActorSettingInfo();
            if (_actorDeSpawnSettingSO) _vanishSetting = _actorDeSpawnSettingSO.CreateActorSettingInfo();

            if (_spawnSetting != null) _spawnSetting.OnConfigure += ConditionToSpawn;
            if (_vanishSetting != null) _vanishSetting.OnConfigure += ConditionToVanish;
        }

        private void OnDisable()
        {
            _onSceneLoadedEventChannel.EventRaised -= ConfigureActors;

            if (_spawnSetting != null) _spawnSetting.OnQuestCompleted -= ActivateSpawnActor;
            if (_vanishSetting != null) _vanishSetting.OnQuestCompleted -= ActivateVanishActor;
        }

        private void ConfigureActors()
        {
            _actor = _actorDef.CreateActor();

            ActorSettingInfo actorSetting = _vanishSetting ?? _spawnSetting;
            IQuestManager.OnConfigureQuest?.Invoke(actorSetting);
        }

        private void ConditionToSpawn(bool isEligible)
        {
            _spawnSetting.OnConfigure -= ConditionToSpawn;

            if (!isEligible)
            {
                _spawnSetting.OnQuestCompleted += ActivateSpawnActor;
                return;
            }

            ActivateSpawnActor();
        }

        private void ConditionToVanish(bool isEligible)
        {
            _vanishSetting.OnConfigure -= ConditionToVanish;

            if (!isEligible)
            {
                InitSpawnSetting();
                _vanishSetting.OnQuestCompleted += ActivateVanishActor;
                return;
            }

            ActivateVanishActor();
        }

        private void InitSpawnSetting()
        {
            if (_spawnSetting != null)
            {
                _vanishSetting.OnQuestCompleted -= ActivateVanishActor;
                IQuestManager.OnConfigureQuest?.Invoke(_spawnSetting);
                return;
            }

            ActivateSpawnActor();
        }

        private void ActivateSpawnActor()
        {
            if (!_spawnPoint) return;

            StartCoroutine(_actor.Spawn(_spawnPoint));
        }

        private void ActivateVanishActor()
        {
            if (!_spawnPoint) return;

            StartCoroutine(_actor.Vanish(gameObject));
        }
    }
}