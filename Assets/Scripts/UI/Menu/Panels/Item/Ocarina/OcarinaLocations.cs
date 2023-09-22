using System.Collections.Generic;
using CryptoQuest.Item.Ocarina;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    public class OcarinaLocations : ScriptableObject
    {
        [field: SerializeField] public List<OcarinaEntrance> Locations = new();
    }
}