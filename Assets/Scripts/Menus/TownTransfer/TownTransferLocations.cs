using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Menus.TownTransfer
{
    public class TownTransferLocations : ScriptableObject
    {
        public List<TownTransferPath> Locations = new();
    }
}