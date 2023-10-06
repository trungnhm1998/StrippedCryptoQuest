using System;
using System.Collections;
using CryptoQuest.Map;
using CryptoQuest.Quest.Authoring;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Actor/TeleportCollideActorSO",
        fileName = "TeleportCollideActor")]
    public class TeleportCollideActorSo : ActorSO<TeleportCollideActorInfo>
    {
        public SceneScriptableObject Destination;
        public MapPathSO Path;

        public override ActorInfo CreateActor() =>
            new TeleportCollideActorInfo(this, QuestData);
    }

    [Serializable]
    public class TeleportCollideActorInfo : ActorInfo<TeleportCollideActorSo>
    {
        public TeleportCollideActorInfo(TeleportCollideActorSo actorSo, QuestSO questData) : base(
            actorSo)
        {
        }

        public TeleportCollideActorInfo()
        {
        }

        public override IEnumerator Spawn(Transform parent)
        {
            AsyncOperationHandle<GameObject> handle =
                Data.Prefab.InstantiateAsync(parent.position, Quaternion.identity);

            yield return handle;
            var actor = handle.Result;
            actor.transform.SetParent(parent);
            GoTo actorConfig = handle.Result.GetComponent<GoTo>();
            actorConfig.SetUpTeleportInfo(Data.Destination, Data.Path);
        }
    }
}