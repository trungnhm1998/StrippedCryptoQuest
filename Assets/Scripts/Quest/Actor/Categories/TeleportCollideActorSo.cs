using System;
using System.Collections;
using CryptoQuest.EditorTool;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/TeleportCollideActorSO",
        fileName = "TeleportCollideActor")]
    public class TeleportCollideActorSo : ActorSO<TeleportCollideActorInfo>
    {
        public SceneScriptableObject Destination;
        public MapPathSO Path;

        public override ActorInfo CreateActor() =>
            new TeleportCollideActorInfo(this);
    }

    [Serializable]
    public class TeleportCollideActorInfo : ActorInfo<TeleportCollideActorSo>
    {
        private AsyncOperationHandle<GameObject> _handle;
        public TeleportCollideActorInfo(TeleportCollideActorSo actorSo) : base(actorSo) { }
        public TeleportCollideActorInfo() { }

        public override IEnumerator Spawn(Transform parent)
        {
            _handle = Data.Prefab.InstantiateAsync(parent.position, parent.rotation, parent);

            yield return _handle;

            GoTo actorConfig = _handle.Result.GetComponent<GoTo>();
            actorConfig.SetUpTeleportInfo(Data.Destination, Data.Path);

            if (!parent.TryGetComponent<ShowCubeWireUtil>(out var showCubeWireUtil)) yield break;
            actorConfig.SetBoxSize(showCubeWireUtil.SizeBox);


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