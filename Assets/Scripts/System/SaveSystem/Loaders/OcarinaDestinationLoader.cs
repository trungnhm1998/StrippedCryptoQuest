using System;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class OcarinaDestinationLoader : Loader
    {
        private const string ObjectName = "Ocarina";
        [SerializeField] private TeleportLocationsSaveSO _locationSave;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override void Load()
        {
            _locationSave.VisitedLocations.Clear();
            if (!_progressionSystem.SaveData.TryGetValue(ObjectName, out var json)) return;
            JsonConvert.PopulateObject(json, _locationSave);
        }
    }

    public class DestinationsLoaded : ActionBase
    {
    }
}