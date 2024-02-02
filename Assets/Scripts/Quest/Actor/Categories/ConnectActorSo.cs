using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

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
        private AsyncOperationHandle<GameObject> _handle;
        public ConnectActorInfo(ConnectActorSo actorSo) : base(actorSo) { }
        public ConnectActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            _handle = Data.Prefab.InstantiateAsync(parent.position, parent.rotation);

            yield return _handle;
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