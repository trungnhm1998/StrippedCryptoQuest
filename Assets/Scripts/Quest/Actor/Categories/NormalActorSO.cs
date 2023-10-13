using System;
using System.Collections;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Actor/NormalActorSO", fileName = "NpcActorDef")]
    public class NormalActorSO : ActorSO<NormalActorInfo>
    {
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
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity, parent);

            yield return handle;
        }
    }
}