using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public abstract class CharacterInformation<TDef, TSpec>
        where TDef : CharacterData<TDef, TSpec>
        where TSpec : CharacterInformation<TDef, TSpec>, new()
    {
        [field: SerializeField] public TDef Data { get; protected set; }
        public virtual void Init(TDef data) => Data = data;
        public virtual bool IsValid() => Data != null;

        public virtual void Release()
        {
            Data = null; // remove ref count from the SO which we dynamically loaded
        }
    }
}