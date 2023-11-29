using System;
using UnityEngine;

namespace CryptoQuest.Character.Beast
{
    [Serializable]
    public class BeastDef
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public int TokenId { get; set; }
        [field: SerializeField] public BeastDataSpec Data { get; set; }
    }
}