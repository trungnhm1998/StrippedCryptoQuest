using System;
using System.Collections;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Actor/NpcActorSO", fileName = "NpcActorDef")]
    public class NpcActorSO : ActorSO<NpcActorInfo>
    {
        public override ActorInfo CreateActor() =>
            new NpcActorInfo(this);
    }

    [Serializable]
    public class NpcActorInfo : ActorInfo<NpcActorSO>
    {
        public NpcActorInfo(NpcActorSO npcActorSO) : base(
            npcActorSO)
        {
        }

        public NpcActorInfo()
        {
        }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity, parent);

            yield return handle;

            QuestGiver questActor = handle.Result.GetComponent<QuestGiver>();
            questActor.SetQuestData(Data.QuestData);

            questActor.GiveQuest();
        }
    }
}