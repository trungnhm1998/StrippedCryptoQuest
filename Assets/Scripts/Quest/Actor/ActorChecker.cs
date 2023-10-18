using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorChecker : MonoBehaviour
    {
        [SerializeField] private ActorSO _actor;
        [SerializeField] private Transform _spawnPoint;

        [Header("Actor Settings")]
        [SerializeField] private ActorSettingSO _actorSpawnAfterDoneSetting;

        [SerializeField] private ActorSettingSO _actorDeSpawnBeforeStart;

        private void OnEnable()
        {
            if (_actorSpawnAfterDoneSetting) _actorSpawnAfterDoneSetting.OnConfigure += Spawn;
            if (_actorDeSpawnBeforeStart) _actorDeSpawnBeforeStart.OnConfigure += DeSpawn;
        }

        private void OnDisable()
        {
            if (_actorSpawnAfterDoneSetting) _actorSpawnAfterDoneSetting.QuestToTrack.OnQuestCompleted -= ActiveActor;
            if (_actorDeSpawnBeforeStart) _actorDeSpawnBeforeStart.QuestToTrack.OnQuestCompleted -= DeActiveActor;
        }

        private void Spawn(bool isCompleted)
        {
            _actorSpawnAfterDoneSetting.OnConfigure -= Spawn;

            if (!isCompleted)
            {
                _actorSpawnAfterDoneSetting.QuestToTrack.OnQuestCompleted += ActiveActor;
                return;
            }

            ActiveActor();
        }

        private void DeSpawn(bool isCompleted)
        {
            _actorDeSpawnBeforeStart.OnConfigure -= DeSpawn;

            if (!isCompleted)
            {
                _actorDeSpawnBeforeStart.QuestToTrack.OnQuestCompleted += DeActiveActor;
                return;
            }

            DeActiveActor();
        }

        private void DeActiveActor()
        {
            if (transform.childCount <= 0) return;
            foreach (Transform child in _spawnPoint)
            {
                Destroy(child);
            }
        }

        private void ActiveActor()
        {
            if (!_actor) return;

            ActorInfo actor = _actor.CreateActor();

            StartCoroutine(actor.Spawn(_spawnPoint));
        }
    }
}