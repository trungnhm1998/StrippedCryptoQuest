using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public class BeastInventorySO : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<IBeast> OwnedBeasts { get; set; } = new();
    }
}