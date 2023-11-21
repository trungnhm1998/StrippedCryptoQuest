using System.Collections.Generic;
using UnityEngine;

namespace TownTransfer
{
    public class TownTransferLocations : ScriptableObject
    {
        public List<TownTransferPath> Locations = new();
    }
}