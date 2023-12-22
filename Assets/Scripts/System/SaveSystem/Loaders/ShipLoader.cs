using System.Collections;
using CryptoQuest.Gameplay.Ship;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class ShipLoader : MonoBehaviour, ILoader
    {
        [SerializeField] private ShipBus _shipBus;

        public IEnumerator Load(SaveSystemSO progressionSystem)
        {
#if UNITY_EDITOR
            _shipBus.IsShipActivated = false;
            _shipBus.HasSailed = false;
#endif
            if (progressionSystem.SaveData.TryGetValue(_shipBus.name, out var json))
                JsonConvert.PopulateObject(json, _shipBus);
            yield break;
        }
    }
}