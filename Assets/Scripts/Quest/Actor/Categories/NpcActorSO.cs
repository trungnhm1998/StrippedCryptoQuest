using System;
using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Actor/NpcActorSO", fileName = "NpcActorDef")]
    public class NpcActorSO : ActorSO<NpcActorInfo>
    {
        public override ActorInfo CreateActor(QuestManager questManager) =>
            new NpcActorInfo(this, QuestData);
    }

    [Serializable]
    public class NpcActorInfo : ActorInfo<NpcActorSO>
    {
        private readonly QuestSO _questData;

        public NpcActorInfo(NpcActorSO npcActorSO, QuestSO questData) : base(
            npcActorSO)
        {
            _questData = questData;
        }

        public NpcActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity);

            yield return handle;

            QuestGiver questActor = handle.Result.GetComponent<QuestGiver>();
            questActor.SetQuestData(_questData);
        }
    }
}