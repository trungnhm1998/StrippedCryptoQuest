using System;
using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor
{
    [CreateAssetMenu(menuName = "Create NpcActorDef", fileName = "NpcActorDef", order = 0)]
    public class NpcActorSO : ActorSO<NpcActorInfo>
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public QuestSO QuestData { get; private set; }

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