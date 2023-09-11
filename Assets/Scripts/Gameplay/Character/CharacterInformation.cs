using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public abstract class CharacterInformation<TDef, TSpec> 
        where TDef : CharacterData<TDef, TSpec> 
        where TSpec : CharacterInformation<TDef, TSpec>, new()
    {
        [field: SerializeField] public TDef Data { get; private set; }
        public void Init(TDef data) => Data = data;
        public bool IsValid() => Data != null;
    }
}