using System;
using CryptoQuest.Gameplay.Ship;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class ShipSaver : SaverBase
    {
        [SerializeField] private ShipBus _shipBus;


        public override void RegistEvents()
        {
            _shipBus.Changed += SaveShipProgression;
        }

        public override void UnregistEvents()
        {
            _shipBus.Changed -= SaveShipProgression;
        }

        private void SaveShipProgression()
        {
            _saveSystem.SaveData[_shipBus.name] = 
                JsonConvert.SerializeObject(_shipBus);
            _saveHandler.Save();
        }
    }
}