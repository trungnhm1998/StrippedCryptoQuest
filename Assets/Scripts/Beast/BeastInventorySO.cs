using System.Collections.Generic;
using CryptoQuest.Beast.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Beast
{
    public class BeastInventorySO : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<IBeast> OwnedBeasts { get; set; } = new();

        public IBeast GetBeast(int id)
        {
            foreach (var beast in OwnedBeasts)
            {
                if (beast.IsValid() && beast.Id == id)
                    return beast;
            }

            return NullBeast.Instance;
        }
    }
}