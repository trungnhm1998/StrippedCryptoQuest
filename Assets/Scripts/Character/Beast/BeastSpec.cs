using System;
using UnityEngine;

namespace CryptoQuest.Character.Beast
{
    [Serializable]
    public class BeastSpec
    {
        [field: SerializeField] public int Id { get; set; }
        
        [field: SerializeField] public float Experience { get; set; }
    }
}