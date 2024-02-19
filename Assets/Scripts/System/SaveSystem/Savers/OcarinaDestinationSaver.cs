using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class OcarinaDestinationSaver : SaverBase
    {
        private const string ObjectName = "Ocarina";
        [SerializeField] private TeleportLocationsSaveSO _saveSo;
        public override void RegistEvents() => _saveSo.Changed += SaveVisitedLocation;
        public override void UnregistEvents() => _saveSo.Changed -= SaveVisitedLocation;


        private void SaveVisitedLocation()
        {
            _saveSystem.SaveData[ObjectName] = JsonConvert.SerializeObject(_saveSo);
        }
    }
}