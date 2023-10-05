using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private ActorSO _actorDef;
        [SerializeField] private Transform _spawnPoint;

        [SerializeField] private QuestManager _questManager;

        private void OnEnable()
        {
            SpawnActor();
        }

        private void SpawnActor()
        {
            ActorInfo actor = _actorDef.CreateActor(_questManager);
            StartCoroutine(actor.Spawn(_spawnPoint));
        }
    }
}