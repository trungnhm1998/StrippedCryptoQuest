using System;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve
{
    [Serializable]
    public struct EvolvableInfoData
    {
        [field: SerializeField] public int EvolveId { get; set; }
        [field: SerializeField] public int Rarity { get; set; }
        [field: SerializeField] public int BeforeStars { get; set; }
        [field: SerializeField] public int AfterStars { get; set; }
        [field: SerializeField] public int Gold { get; set; }
        [field: SerializeField] public float Metad { get; set; }
        [field: SerializeField] public int Rate { get; set; }
    }
}
