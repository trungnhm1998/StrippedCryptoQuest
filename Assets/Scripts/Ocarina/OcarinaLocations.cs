using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Ocarina
{
    public class OcarinaLocations : ScriptableObject
    {
        [field: SerializeField] public List<OcarinaEntrance> Locations = new();

        [field: SerializeField] public List<OcarinaEntrance> ConditionalLocations = new();
    }
}