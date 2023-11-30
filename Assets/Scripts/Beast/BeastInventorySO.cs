using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public class BeastInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<Beast> OwnedBeasts { get; set; } = new();
    }
}