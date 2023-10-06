using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.Quest.Actor
{
    [Serializable]
    public abstract class ActorInfo
    {
        public abstract bool IsValid();

        public abstract IEnumerator Spawn(Transform parent);
    }

    [Serializable]
    public abstract class ActorInfo<TDef> : ActorInfo where TDef : ActorSO
    {
        [field: SerializeField] public TDef Data { get; protected set; }
        protected ActorInfo(TDef data) => Data = data;
        protected ActorInfo() { }
        public override bool IsValid() => Data != null;
    }
}