using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/TriggerZoneActorSO", fileName = "TriggerZoneActorSO")]
    public class TriggerZoneActorSO : ActorSO<TriggerZoneActorInfo>
    {
        [field: SerializeField] public Vector2 SizeBox { get; private set; }
        public override ActorInfo CreateActor() => new TriggerZoneActorInfo(this);
    }

    public class TriggerZoneActorInfo : ActorInfo<TriggerZoneActorSO>
    {
        private AsyncOperationHandle<GameObject> _handle;
        public TriggerZoneActorInfo(TriggerZoneActorSO triggerZoneActorSO) : base(triggerZoneActorSO) { }
        public TriggerZoneActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            _handle = Data.Prefab.InstantiateAsync(parent.position, parent.rotation, parent);

            yield return _handle;

            if (!_handle.Result.TryGetComponent<BoxCollider2D>(out var targetCollider2D)) yield break;
            if (!parent.TryGetComponent<BoxCollider2D>(out var parentCollider2D)) yield break;

            targetCollider2D.size = parentCollider2D.size;
            targetCollider2D.isTrigger = parentCollider2D.isTrigger;
        }

        public override IEnumerator Vanish(GameObject parent)
        {
            if (!_handle.IsValid())
            {
                Object.Destroy(parent);
                yield break;
            }

            yield return _handle;

            if (!parent.TryGetComponent<ActorSpawner>(out var actorSpawner)) yield break;

            Object.Destroy(actorSpawner.gameObject);
        }
    }
}