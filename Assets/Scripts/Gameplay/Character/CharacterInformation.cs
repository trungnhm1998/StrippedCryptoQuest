using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public class CharacterInformation<TData, TInfo> 
        where TData : CharacterData<TData, TInfo> 
        where TInfo : CharacterInformation<TData, TInfo>, new()
    {
        [field: SerializeField] public TData Data { get; private set; }
        public void Init(TData data) => Data = data;
        public bool IsValid() => Data != null;
    }
}