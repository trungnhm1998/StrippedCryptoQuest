using System.Collections;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Actor/TriggerZoneActorSO", fileName = "TriggerZoneActorSO")]
    public class TriggerZoneActorSO : ActorSO<TriggerZoneActorInfo>
    {
        [field: SerializeField] public Vector2 SizeBox { get; private set; }
        public override ActorInfo CreateActor() => new TriggerZoneActorInfo(this);
    }

    public class TriggerZoneActorInfo : ActorInfo<TriggerZoneActorSO>
    {
        private GameObject _gameObject;

        public TriggerZoneActorInfo(TriggerZoneActorSO triggerZoneActorSO) : base(triggerZoneActorSO)
        {
        }

        public TriggerZoneActorInfo()
        {
        }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity, parent);

            yield return handle;

            _gameObject = handle.Result;

            QuestColliderTrigger colliderTrigger = handle.Result.GetComponent<QuestColliderTrigger>();
            colliderTrigger.SetColliderBoxSize(Data.SizeBox);
            colliderTrigger.SetQuestData(Data.QuestData);

            Data.QuestData.OnQuestCompleted += CompleteQuestHandle;
        }

        private void CompleteQuestHandle()
        {
            Object.Destroy(_gameObject);

            Data.QuestData.OnQuestCompleted -= CompleteQuestHandle;
        }
    }
}