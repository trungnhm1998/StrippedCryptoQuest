using System;
using System.Collections;
using CryptoQuest.Gameplay.Ship;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class ShipLoader : Loader
    {
        [SerializeField] private ShipBus _shipBus;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override void Load()
        {
#if UNITY_EDITOR
            _shipBus.IsShipActivated = false;
            _shipBus.HasSailed = false;
#endif
            if (_progressionSystem.SaveData.TryGetValue(_shipBus.name, out var json))
                JsonConvert.PopulateObject(json, _shipBus);
        }
    }
}