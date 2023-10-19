using System;
using System.Collections;
using CryptoQuest.Character;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/NormalActorSO", fileName = "NormalActorSO")]
    public class NormalActorSO : ActorSO<NormalActorInfo>
    {
        [field: SerializeField] public string Name { get; private set; }

        [field: Header("Config Settings")]
        [field: SerializeField] public QuestSO Quest { get; private set; }

        public override ActorInfo CreateActor() => new NormalActorInfo(this);
    }

    [Serializable]
    public class NormalActorInfo : ActorInfo<NormalActorSO>
    {
        public NormalActorInfo(NormalActorSO npcActorSO) : base(npcActorSO) { }
        public NormalActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, parent.rotation, parent);

            yield return handle;

            if (Data.Quest == null) yield break;

            NPCBehaviour npcBehaviour = parent.GetComponentInChildren<NPCBehaviour>();

            if (!npcBehaviour.TryGetComponent<QuestGiver>(out var questGiver)) yield break;

            questGiver.Init(Data.Quest);
        }
    }
}