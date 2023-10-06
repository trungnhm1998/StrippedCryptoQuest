using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private ActorSO _actorDef;
        [SerializeField] private Transform _spawnPoint;

        private void OnEnable()
        {
            SpawnActor();
        }

        private void SpawnActor()
        {
            ActorInfo actor = _actorDef.CreateActor();
            StartCoroutine(actor.Spawn(_spawnPoint));
        }
    }
}