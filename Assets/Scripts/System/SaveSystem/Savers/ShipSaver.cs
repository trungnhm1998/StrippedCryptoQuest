using CryptoQuest.Gameplay.Ship;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class ShipSaver : MonoBehaviour
    {
        [SerializeField] private ShipBus _shipBus;
        [SerializeField] private SaveSystemSO _saveSystem;

        private void OnEnable()
        {
            _shipBus.Changed += SaveShipProgression;
        }

        private void OnDisable()
        {
            _shipBus.Changed -= SaveShipProgression;
        }

        private void SaveShipProgression()
        {
            _saveSystem.SaveData[_shipBus.name] = 
                JsonConvert.SerializeObject(_shipBus);
            _saveSystem.Save();
        }
    }
}