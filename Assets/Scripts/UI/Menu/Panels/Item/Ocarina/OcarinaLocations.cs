using System.Collections.Generic;
using CryptoQuest.Item.Ocarina;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    [CreateAssetMenu(menuName = "Create OcarinaLocations", fileName = "OcarinaLocations", order = 0)]
    public class OcarinaLocations : ScriptableObject
    {
        [field: SerializeField] public List<OcarinaEntrance> Locations = new();
    }
}