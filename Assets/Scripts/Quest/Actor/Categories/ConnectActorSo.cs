using System;
using System.Collections;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/ConnectActorSO", fileName = "ConnectActor")]
    public class ConnectActorSo : ActorSO<ConnectActorInfo>
    {
        public override ActorInfo CreateActor() =>
            new ConnectActorInfo(this);
    }

    [Serializable]
    public class ConnectActorInfo : ActorInfo<ConnectActorSo>
    {
        public ConnectActorInfo(ConnectActorSo actorSo) : base(actorSo) { }
        public ConnectActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, parent.rotation);

            yield return handle;
        }
    }
}