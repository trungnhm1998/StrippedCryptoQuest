using System;
using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actor.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Actor/Void Event Actor", fileName = "VoidEventActorSO")]
    public class VoidEventActorSO : ActorSO<VoidEventActorInfo>
    {
        [field: SerializeField]
        public VoidEventChannelSO[] Events { get; private set; }

        public override ActorInfo CreateActor() => new VoidEventActorInfo(this);
    }

    [Serializable]
    public class VoidEventActorInfo : ActorInfo<VoidEventActorSO>
    {
        private VoidEventActorSO _actorSO;

        public VoidEventActorInfo() { }
        public VoidEventActorInfo(VoidEventActorSO actorSO) : base(actorSO)
        {
            _actorSO = actorSO;
        }

        public override IEnumerator Spawn(Transform parent)
        {
            foreach (var eventSO in _actorSO.Events)
            {
                eventSO.RaiseEvent();
            }
            yield break;
        }

        public override IEnumerator Vanish(GameObject parent)
        {
            yield break;
        }
    }
}