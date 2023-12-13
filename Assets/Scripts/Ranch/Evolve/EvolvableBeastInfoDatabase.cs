using System;
using UnityEngine;

namespace CryptoQuest.Ranch.Evolve
{
    [Serializable]
    public class EvolvableBeastInfoDatabase
    {
        [field: SerializeField] public int BeastEvolId { get; set; }
        [field: SerializeField] public int BeforeStars { get; set; }
        [field: SerializeField] public int AfterStars { get; set; }
        [field: SerializeField] public int Rate { get; set; }
        [field: SerializeField] public int Gold { get; set; }
        [field: SerializeField] public float Metad { get; set; }
    }
}