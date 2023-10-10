using System.Collections;
using CryptoQuest.Quest.Components;
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
        private GameObject _gameObject;
        public TriggerZoneActorInfo(TriggerZoneActorSO triggerZoneActorSO) : base(triggerZoneActorSO) { }
        public TriggerZoneActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity, parent);

            yield return handle;

            _gameObject = handle.Result;

            QuestColliderTrigger colliderTrigger = handle.Result.GetComponent<QuestColliderTrigger>();
            colliderTrigger.Init(Data.QuestData, Data.SizeBox);

            Data.QuestData.OnQuestCompleted += CompleteQuestHandle;

            if (!handle.Result.TryGetComponent<BoxCollider2D>(out var targetCollider2D)) yield break;
            if (!parent.TryGetComponent<BoxCollider2D>(out var parentCollider2D)) yield break;

            targetCollider2D.size = parentCollider2D.size;
            targetCollider2D.isTrigger = parentCollider2D.isTrigger;
        }

        private void CompleteQuestHandle()
        {
            Object.Destroy(_gameObject);

            Data.QuestData.OnQuestCompleted -= CompleteQuestHandle;
        }
    }
}